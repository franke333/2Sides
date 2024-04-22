using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCartScript : SingletonClass<ShoppingCartScript>, IInteractable
{
    [SerializeField]
    private GameObject _handleGO;
    private MeshRenderer _handleMR;
    private Color _handleColor;

    private void Start()
    {
        _handleMR = _handleGO?.GetComponent<MeshRenderer>();
        if(_handleMR != null)
            _handleColor = _handleMR.material.color;
    }

    public void HoverOver()
    {
        if(_handleMR != null)
        {
            _handleMR.material.color = Color.green;
        }
    }

    public void HoverOut()
    {
        if(_handleMR != null)
        {
            _handleMR.material.color = _handleColor;
        }
    }

    public void Interact(bool value)
    {
        if(value)
        {
            Debug.Log("Shopping cart is being pushed");
        }
        else
        {
            Debug.Log("Shopping cart is not being pushed");
        }
    }
}
