using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ItemScript : MonoBehaviour, IInteractable
{
    public string itemName;

    MeshRenderer[] _meshRenderers;
    HighlightScript _highlightScript;

    private GameObject env;


    private void Start()
    {
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();

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
        var DEBUG_CART = PlayerController.Instance.DEBUG_CART;
        // Left mouse hold -> pick an item with the left arm
        if(value)
        {
            //nic
        }

        //transform.position = DEBUG_CART.transform.position + Vector3.up * 2;
    }
    public void Touch(bool value, Transform h)
    {
        if (value)
        {
            Debug.Log("chytl jsem item");
            Physics.IgnoreCollision(PlayerController.Instance.Body.GetComponent<Collider>(), transform.GetComponent<Collider>(), true);
            transform.GetComponent<Rigidbody>().isKinematic = true;
            transform.parent = h.transform;
            transform.position = h.transform.position;
            return;
        }
        else
        {
            Debug.Log("Pustil jsem item");
            transform.parent = null;
            transform.GetComponent<Rigidbody>().isKinematic = false;
            Physics.IgnoreCollision(PlayerController.Instance.Body.GetComponent<Collider>(), transform.GetComponent<Collider>(), false);
        }
    }
}