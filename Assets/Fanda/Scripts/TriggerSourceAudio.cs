using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TriggerSourceAudio : MonoBehaviour, IInteractable
{
    public AudioClip audioClip;
    public float range;
    public float maxTriggerPerSecond = 33f;

    private AudioSource _audioSource;
    private PlayerController _player;

    private Transform[] _childObjects;
    private Vector3[] _directions;
    private bool _punching = false;

    private void Start()
    {
        _player = PlayerController.Instance;
        _audioSource = gameObject.GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogWarning("AudioSource not found. Adding AudioSource component to the GameObject.");
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        _audioSource.clip = audioClip;

        _childObjects = GetComponentsInChildren<Transform>();
        _directions = new Vector3[_childObjects.Length];
        for (int i = 0; i < _childObjects.Length; i++)
        {
            _directions[i] = UnityEngine.Random.insideUnitSphere;
        }

    }


    private void Update()
    {
        if (Vector3.Distance(_player.transform.position,transform.position) > range)
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
            return;
        }

        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }

        float applyTrigger = math.lerp(0,maxTriggerPerSecond,1-Vector3.Distance(_player.transform.position,transform.position)/range);
        TriggerMeter.Instance.ChangeValue(applyTrigger * Time.deltaTime);
        if (_punching)
        {
            for (int i = 0; i < _childObjects.Length; i++)
            {
                _childObjects[i].position += _directions[i] * Time.deltaTime * 3;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void HoverOver()
    {
    }

    public void HoverOut() { 
    }

    public void Interact(bool value)
    {
        _punching = true;
    }
}
