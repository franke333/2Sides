using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MopScript : MonoBehaviour, IInteractable
{
    public int throwForce;
    PlayerCameraScript _playerCameraScript;

    public bool cleaning = false;

    private int rememberVelocityQueue = 4;
    private float importanceMultiplier = 0.9f;
    private Queue<Vector3> positionsQueue;
    private float velocityMultiplier = 25f;

    private void AddRetainedVelocityOnLetGo()
    {
        Vector3 sum = Vector3.zero;
        float multiplier = 1f;
        float multSum = 0;
        Vector3 lastPos = transform.position;
        positionsQueue.Enqueue(lastPos);
        foreach (var pos in positionsQueue.Reverse())
        {
            sum += (lastPos - pos) * multiplier;
            Debug.Log("offset: " + (lastPos - pos));
            multSum += multiplier;
            multiplier *= importanceMultiplier;
            lastPos = pos;

        }
        Debug.Log("sum: " + sum);
        GetComponent<Rigidbody>().AddForce(sum * velocityMultiplier / multSum, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        positionsQueue.Enqueue(transform.position);
        if (positionsQueue.Count > rememberVelocityQueue)
            positionsQueue.Dequeue();
    }

    private void Start()
    {
        positionsQueue = new Queue<Vector3>();
        _playerCameraScript = FindObjectOfType<PlayerCameraScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!cleaning)
            return;
        if (other.gameObject.CompareTag("Honey"))
        {
            Destroy(other.gameObject);
            PlayerController.Instance.RemoveSpeedModifier("Honey");
        }
        if (other.gameObject.CompareTag("Water"))
        {
            Destroy(other.gameObject);
        }
    }

    public void HoverOver()
    {
    }

    public void HoverOut()
    {
    }

    public void InteractView(bool value)
    {
    }

    public void Touch(bool value, Transform h)
    {
        if (value)
        {
            foreach (var collider in GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(PlayerController.Instance.Body.GetComponent<Collider>(), collider, true);
                Physics.IgnoreCollision(PlayerController.Instance.Head.GetComponent<Collider>(), collider, true);
            }
            transform.GetComponent<Rigidbody>().isKinematic = true;
            transform.parent = h.transform;
            transform.position = h.transform.position;
            return;
        }
        else
        {
            transform.parent = null;
            transform.GetComponent<Rigidbody>().isKinematic = false;
            AddRetainedVelocityOnLetGo();
            foreach (var collider in GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(PlayerController.Instance.Body.GetComponent<Collider>(), collider, false);
                Physics.IgnoreCollision(PlayerController.Instance.Head.GetComponent<Collider>(), collider, false);
            }
        }
    }

    public void Throw()
    {
        StartCoroutine(Untouchable());
        transform.parent = null;
        transform.GetComponent<Rigidbody>().isKinematic = false;
        transform.GetComponent<Rigidbody>().AddForce(_playerCameraScript.GetViewVector() * throwForce);
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            Physics.IgnoreCollision(PlayerController.Instance.Body.GetComponent<Collider>(), collider, false);
            Physics.IgnoreCollision(PlayerController.Instance.Head.GetComponent<Collider>(), collider, false);
        }
    }

    private IEnumerator Untouchable()
    {
        float countDown = 1f;
        while (countDown >= 0)
        {
            Physics.IgnoreLayerCollision(3, 6, true); // Ignore collisions for item and player so it could fly away
            countDown -= Time.deltaTime;
            yield return null;
        }
        Physics.IgnoreLayerCollision(3, 6, false);
    }
}
