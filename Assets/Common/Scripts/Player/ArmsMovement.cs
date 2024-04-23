using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class ArmsMovement : MonoBehaviour
{
    private PlayerCameraScript _playerCameraScript;
    private Vector3 mousePos;
    private Vector3 direction;
    private Quaternion targetRotation;
    private Quaternion defaultPosL;
    private Quaternion defaultPosR;

    private void Start()
    {
        _playerCameraScript = FindObjectOfType<PlayerCameraScript>();
        if (transform.name == "LeftArmPivot")
        {
            defaultPosL = transform.rotation;
        }
        else defaultPosR = transform.rotation;
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
                transform.rotation = targetRotation;
            }
        }

        GoBack();
    }

    void GoBack()
    {
        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1) && transform.name == "RightArmPivot")
        {
            transform.localRotation = Quaternion.Euler(80.7f, 0f, 0f);
        }
        else if (!Input.GetMouseButton(0) && Input.GetMouseButton(1) && transform.name == "LeftArmPivot")
        {
            transform.localRotation = Quaternion.Euler(80.7f, 0f, 0f);
        }
        else if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            transform.localRotation = Quaternion.Euler(80.7f, 0f, 0f);
        }
    }
}
