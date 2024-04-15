using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SingletonClass<PlayerController>
{
    private PlayerMovementScript _playerMovementScript;
    private PlayerCameraScript _playerCameraScript;

    public float maxPickupDistance = 3;
    public LayerMask interactableLayer;

    private IInteractable _currentInteractible;

    public GameObject DEBUG_CART;

    private void Start()
    {
        _playerMovementScript = GetComponent<PlayerMovementScript>();
        _playerCameraScript = GetComponentInChildren<PlayerCameraScript>();
    }

    private void HoverOverCheck()
    {
        //find which item is in front of the player
        RaycastHit hit;
        if (Physics.Raycast(_playerCameraScript.GetCameraPos(), _playerCameraScript.GetViewVector(), out hit, maxPickupDistance, interactableLayer))
        {
            var item = hit.collider.GetComponentInParent<IInteractable>(); 
            if (item != null)
            {
                if (item != _currentInteractible)
                {
                    _currentInteractible?.HoverOut();
                    _currentInteractible = item;
                    item.HoverOver();
                }
                return;
            }
        }
         _currentInteractible?.HoverOut();
        _currentInteractible = null;
    }

    private void PlayerInput()
    {
        foreach(var value in new bool[]{true, false})
            if(Input.GetMouseButtonDown(value ? 0 : 1) && _currentInteractible != null)
                _currentInteractible.Interact(value);
    }

    private void Update()
    {
        HoverOverCheck();
        PlayerInput();
    }

    private void OnDrawGizmos()
    {
        if (_playerMovementScript == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_playerCameraScript.GetCameraPos(),_playerCameraScript.GetViewVector()* maxPickupDistance);
    }
}