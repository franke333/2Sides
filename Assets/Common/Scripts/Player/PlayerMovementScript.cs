using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    Rigidbody _rb;

    public float maxSpeed = 1f;
    public float acceleration = 1f;
    public float drag = 0.99f;

    public float runSpeed = 1.5f;

    public float verticalInput { get; private set; }
    public float horizontalInput { get; private set; }

    private bool _sprinting = false;
    private bool _crouching = false;

    [SerializeField]
    float _crouchYOffset = 0.5f;

    [SerializeField]
    GameObject _standingBody, _crouchingBody;

    private GameObject _headGO;

    void LoadComponents()
    {
        _rb = GetComponent<Rigidbody>();
        _headGO = transform.Find("Head").gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadComponents();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        _sprinting = Input.GetKey(KeyCode.LeftShift);
        _crouching = Input.GetKey(KeyCode.LeftControl);
        
    }

    void ApplyFixedMovement(Vector3 direction)
    {
        _rb.AddRelativeForce(direction*acceleration*_rb.mass);

        var sprintMultiplier = _sprinting ? runSpeed : 1;
        _rb.maxLinearVelocity = maxSpeed * sprintMultiplier;

    }

    void ProcessCrouching()
    {
        if(_crouching == _crouchingBody.activeSelf)
        {
            return;
        }

        if(_crouching)
        {
            _standingBody.SetActive(false);
            _crouchingBody.SetActive(true);
            transform.position -= Vector3.up * _crouchYOffset;
        }
        else
        {
            _standingBody.SetActive(true);
            _crouchingBody.SetActive(false);
            transform.position += Vector3.up * _crouchYOffset;
        }
    }

    private void FixedUpdate()
    {
        ProcessCrouching();
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        ApplyFixedMovement(direction);
    }
}
