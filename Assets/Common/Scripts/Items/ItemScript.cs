using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ItemScript : MonoBehaviour, IInteractable
{
    public string itemName;
    public int throwForce;

    private PlayerCameraScript _playerCameraScript;

    MeshRenderer[] _meshRenderers;
    HighlightScript _highlightScript;


    private void Start()
    {
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();

        _playerCameraScript = FindObjectOfType<PlayerCameraScript>();

        _highlightScript = gameObject.AddComponent<HighlightScript>();
        _highlightScript.Init(_meshRenderers, Color.yellow);
    }

    //Do this in Manager
    public static ItemScript currentHoverOver;

    public void HoverOver()
    {
        _highlightScript.Highlight();
    }

    public void HoverOut()
    {
        _highlightScript.RemoveHighlight();
    }

    public void InteractView(bool value)
    {
        throw new System.NotImplementedException();
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
                Physics.IgnoreCollision(PlayerController.Instance.Body.GetComponent<Collider>(), collider, true);
                Physics.IgnoreCollision(PlayerController.Instance.Head.GetComponent<Collider>(), collider, true);
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
            Physics.IgnoreCollision(PlayerController.Instance.Body.GetComponent<Collider>(), collider, true);
            Physics.IgnoreCollision(PlayerController.Instance.Head.GetComponent<Collider>(), collider, true);
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