using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{
    public float sensX;
    public float sensY;

    [Range(0, 90)]
    public float xClamp;

    float xRotation = 0;
    float yRotation = 0;

    private GameObject _body;

    public Vector3 GetCameraPos() => transform.position;
    public Vector3 GetViewVector() => transform.forward;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _body = transform.parent.gameObject;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        _body.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
