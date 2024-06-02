using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AudioOnMovementScript : MonoBehaviour
{
    [SerializeField]
    float maxVolume = 1f, minVolume = 0f;
    [SerializeField]
    float maxSpeed = 1f, minSpeed = 0f;
    [SerializeField]
    AnimationCurve volumeCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField]
    AudioClip audioClip;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    NavMeshAgent navMeshAgent;


    private void Start()
    {
        audioSource ??= gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.volume = minVolume;
        audioSource.Play();
    }

    private void Update()
    {
        float speed = 0;
        if (rb != null)
        {
            speed = rb.velocity.magnitude;
        }
        else if (navMeshAgent != null)
        {
            speed = navMeshAgent.velocity.magnitude;
        }
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        float volume = volumeCurve.Evaluate(Mathf.InverseLerp(minSpeed, maxSpeed, speed));
        audioSource.volume = Mathf.Lerp(minVolume, maxVolume, volume);
    }

    

}
