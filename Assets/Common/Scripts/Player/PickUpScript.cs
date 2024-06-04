using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    private IInteractable _currentInteractible;

    private TrajectoryScript _trajectoryScript;

    private bool touched = false;

    private Transform holdPos;
    public Transform HoldPosLeft;
    public Transform HoldPosRight;

    private bool rightArmItem;

    private GameObject _currentItem;

    private void Start()
    {
        _trajectoryScript = GetComponent<TrajectoryScript>();
    }

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
            _currentItem = col.gameObject;
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
            _trajectoryScript.SetObjectToThrow(null);
        }
    }

    public void ThrowItem()
    {
        if (Input.GetKeyDown(KeyCode.Q) && rightArmItem == false)
        {
            _trajectoryScript.SetObjectToThrow(_currentItem.GetComponent<Rigidbody>());
        }
        if (Input.GetKeyUp(KeyCode.Q) && rightArmItem == false)
        {
            touched = false;
            _currentInteractible.Throw();
            _trajectoryScript.SetObjectToThrow(null);
        }
        if (Input.GetKeyUp(KeyCode.E) && rightArmItem == true)
        {
            touched = false;
            _currentInteractible.Throw();
        }
    }
}
