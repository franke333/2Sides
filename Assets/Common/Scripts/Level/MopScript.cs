using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MopScript : MonoBehaviour, IInteractable
{
    public int throwForce;
    PlayerCameraScript _playerCameraScript;
    private void Start()
    {
        _playerCameraScript = FindObjectOfType<PlayerCameraScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Honey"))
        {
            Destroy(other.gameObject);
            PlayerController.Instance.RemoveSpeedModifier("Honey");
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
