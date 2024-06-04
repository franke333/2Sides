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

    [SerializeField]
    GameObject _arm;

    public float extensionLength = 1.5f;
    public float retracedLength = 0f;
    public float extensionTime = 1f;
    public float retractTime = 1f;
    public AnimationCurve extensionCurve;

    private bool handHeldUp = false;
    private float currentExtension = 0f;
    private float offset = 0;
    private Vector3 og_position;

    private void Start()
    {
        _pickUp = GetComponentInChildren<PickUpScript>();
        _playerCameraScript = FindObjectOfType<PlayerCameraScript>();
        transform.GetComponentInChildren<Collider>().enabled = false;
        og_position = _arm.transform.localPosition;
    }

    private void Update()
    {
        mousePos = _playerCameraScript.GetViewVector();
        direction = mousePos;
        targetRotation = Quaternion.LookRotation(direction);
        handHeldUp = Input.GetMouseButton(RightArm ? 1 : 0);
        if (handHeldUp)
        {

            if(!RightArm)
                transform.GetComponentInChildren<Collider>().enabled = true;
            if(RightArm)
                _arm.transform.localPosition = og_position - Vector3.forward * 0.5f;
            transform.rotation = targetRotation;
        }
        else if(RightArm)
            _arm.transform.localPosition = og_position;

        GoBack();
        ExtendingLeftArm();
    }

    private void ExtendingLeftArm()
    {
        if(RightArm)
            return;
        if(!handHeldUp)
        {
            currentExtension = 0;
            _arm.transform.localPosition = og_position;
            return;
        }
        if(Input.GetKey(KeyCode.LeftShift))
            currentExtension += Time.deltaTime / extensionTime;
        else
            currentExtension -= Time.deltaTime / retractTime;
        currentExtension = Mathf.Clamp(currentExtension, 0, 1);
        offset = Mathf.Lerp(retracedLength, extensionLength, extensionCurve.Evaluate(currentExtension));
        _arm.transform.localPosition = Vector3.forward * offset + og_position;

        
    }

    void GoBack()
    {
        if (!handHeldUp)
        {
            _pickUp.DropItem();
            transform.localRotation = Quaternion.Euler(80.7f, 0f, 0f);
            if(!RightArm)
                transform.GetComponentInChildren<Collider>().enabled = false;
        }
    }
}
