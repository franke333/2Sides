using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsMovement : MonoBehaviour
{
    public GameObject RightArm;
    public GameObject LeftArm;

    private PlayerCameraScript _playerCameraScript;
    private Vector3 mousePos;
    private Vector3 direction;
    private Quaternion targetRotation;
    private PickUpScript _pickUp;

    private void Start()
    {
        _pickUp = GetComponentInChildren<PickUpScript>();
        _playerCameraScript = FindObjectOfType<PlayerCameraScript>();
        RightArm.GetComponent<Collider>().enabled = false;
        LeftArm.GetComponent<Collider>().enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            mousePos = _playerCameraScript.GetViewVector();
            direction = mousePos;
            targetRotation = Quaternion.LookRotation(direction);

            if (transform.name == "LeftArmPivot")
            {
                LeftArm.GetComponent<Collider>().enabled = true;
                transform.rotation = targetRotation;
            }
        }
        if (Input.GetMouseButton(1))
        {
            mousePos = _playerCameraScript.GetViewVector();
            direction = mousePos;
            targetRotation = Quaternion.LookRotation(direction);

            if (transform.name == "RightArmPivot")
            {
                RightArm.GetComponent<Collider>().enabled = true;
                transform.rotation = targetRotation;
            }
        }

        GoBack();
    }

    void GoBack()
    {
        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1) && transform.name == "RightArmPivot")
        {
            _pickUp.DropItem();
            transform.localRotation = Quaternion.Euler(80.7f, 0f, 0f);
            RightArm.GetComponent<Collider>().enabled = false;
        }
        else if (!Input.GetMouseButton(0) && Input.GetMouseButton(1) && transform.name == "LeftArmPivot")
        {
            _pickUp.DropItem();
            transform.localRotation = Quaternion.Euler(80.7f, 0f, 0f);
            LeftArm.GetComponent<Collider>().enabled = false;
        }
        else if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            _pickUp.DropItem();
            transform.localRotation = Quaternion.Euler(80.7f, 0f, 0f);
            RightArm.GetComponent<Collider>().enabled = false;
            LeftArm.GetComponent<Collider>().enabled = false;
        }
    }
}
