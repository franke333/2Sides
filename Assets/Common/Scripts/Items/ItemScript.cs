using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ItemScript : MonoBehaviour, IInteractable
{
    public string itemName;

    MeshRenderer[] _meshRenderers;
    HighlightScript _highlightScript;



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

    public void Interact(bool value)
    {
        var DEBUG_CART = PlayerController.Instance.DEBUG_CART;
        //left click -> push item in random direction
        if(value)
        {
            GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * Random.Range(2, 15), ForceMode.Impulse);
            return;
        }
        //right click -> move item to cart
        transform.position = DEBUG_CART.transform.position + Vector3.up * 2;
    }
}
