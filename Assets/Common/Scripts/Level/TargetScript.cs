using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetScript : MonoBehaviour
{
    public UnityEvent onHitEvent;

    bool invoked = false;

    [Header("Slide Settings")]
    public Vector3 slideDirection;
    public float slideSpeed;
    public float destroyAfter;

    float timer = 0;

    private void Update()
    {
        if(invoked)
        {
            timer += Time.deltaTime;
            if(timer >= destroyAfter)
            {
                Destroy(gameObject);
            }
            Slide();
        }
    }

    public void Slide()
    {
        transform.position += slideDirection.normalized * slideSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(invoked) return;
        if(other.CompareTag("Item"))
        {
            onHitEvent.Invoke();
            invoked = true;
        }
    }
}
