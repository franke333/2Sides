using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsMovement : MonoBehaviour
{
    public bool RightArm;

    private PlayerCameraScript _playerCameraScript;
    private Vector3 mousePos;
    private Vector3 direction;
    private Quaternion targetRotation;
    private PickUpScript _pickUp;

    private void Start()
    {
        _pickUp = GetComponentInChildren<PickUpScript>();
        _playerCameraScript = FindObjectOfType<PlayerCameraScript>();
        transform.GetComponentInChildren<Collider>().enabled = false;
    }

    private void Update()
    {
        mousePos = _playerCameraScript.GetViewVector();
        direction = mousePos;
        targetRotation = Quaternion.LookRotation(direction);

        if (Input.GetMouseButton(RightArm ? 1 : 0))
        {
            if(!RightArm)
                transform.GetComponentInChildren<Collider>().enabled = true;
            transform.rotation = targetRotation;
        }

        GoBack();
    }

    void GoBack()
    {
        if (!Input.GetMouseButton(RightArm ? 1 : 0))
        {
            _pickUp.DropItem();
            transform.localRotation = Quaternion.Euler(80.7f, 0f, 0f);
            if(!RightArm)
                transform.GetComponentInChildren<Collider>().enabled = false;
        }
    }
}
