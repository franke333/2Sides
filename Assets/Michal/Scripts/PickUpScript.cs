using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    private IInteractable _currentInteractible;

    public bool touched = false;

    private Transform holdPos;
    public Transform HoldPosLeft;
    public Transform HoldPosRight;

    private void Update()
    {
        if (touched)
        {
            _currentInteractible.Touch(touched, holdPos);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 6 && touched == false)
        {
            if (transform.name == "LeftArm") holdPos = HoldPosLeft;
            else if (transform.name == "RightArm") holdPos = HoldPosRight;
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
}
