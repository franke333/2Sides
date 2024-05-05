using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    private IInteractable _currentInteractible;

    private bool touched = false;

    private Transform holdPos;
    public Transform HoldPosLeft;
    public Transform HoldPosRight;

    private bool rightArmItem;

    private void Update()
    {
        if (touched)
        {
            ThrowItem();
            _currentInteractible.Touch(touched, holdPos);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        // 6 == layer.pickable
        if (col.gameObject.layer == 6 && touched == false)
        {
            Debug.Log("Touched " + col.name);
            if (transform.name == "LeftArm")
            {
                holdPos = HoldPosLeft;
                rightArmItem = false;
            }
            else if (transform.name == "RightArm")
            {
                holdPos = HoldPosRight;
                rightArmItem = true;
            }
            var item = col.GetComponentInParent<IInteractable>();
            _currentInteractible = item;
            touched = true;
            return;
        }
    }

    public void DropItem()
    {
        if (touched)
        {
            touched = false;
            _currentInteractible.Touch(touched, holdPos);
        }
    }

    public void ThrowItem()
    {
        if (Input.GetKeyDown(KeyCode.Q) && rightArmItem == false)
        {
            touched = false;
            _currentInteractible.Throw();
        }
        if (Input.GetKeyUp(KeyCode.E) && rightArmItem == true)
        {
            touched = false;
            _currentInteractible.Throw();
        }
    }
}
