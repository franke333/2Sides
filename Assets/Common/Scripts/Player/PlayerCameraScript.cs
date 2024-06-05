using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : SingletonClass<PlayerCameraScript>
{
    public float sensX;
    public float sensY;

    [Range(0, 90)]
    public float xClamp;

    float xRotation = 0;
    float yRotation = 0;

    private GameObject _body;
    private Rigidbody _bodyRB;
    private Camera camera;
    public GameObject _cameraPoint;

    public Vector3 GetCameraPos() => transform.position;
    public Vector3 GetViewVector() => transform.forward;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _body = transform.parent.gameObject;
        _bodyRB = _body.GetComponent<Rigidbody>();
        _bodyRB.maxAngularVelocity = 0;
        camera = Camera.main;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime * SettingsManager.Instance.MouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime * SettingsManager.Instance.MouseSensitivity;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //_body.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        _bodyRB.MoveRotation(Quaternion.Euler(0, yRotation, 0));
        camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        camera.transform.position = _cameraPoint.transform.position;

 
    }

    private void FixedUpdate()
    {
        _bodyRB.MoveRotation(Quaternion.Euler(0, yRotation, 0));
    }
}
