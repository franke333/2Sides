using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    Rigidbody _rb;

    public float maxSpeed = 1f;
    public float acceleration = 1f;
    public float drag = 0.99f;

    public float verticalInput { get; private set; }
    public float horizontalInput { get; private set; }

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
    }

    void ApplyFixedMovement(Vector3 direction)
    {
        if(direction.magnitude == 0)
        {
            _rb.velocity *= drag * Time.fixedDeltaTime;
            return;
        }
        _rb.AddRelativeForce(direction*acceleration*_rb.mass);
        if (_rb.velocity.magnitude > maxSpeed)
        {
            _rb.velocity = _rb.velocity.normalized * maxSpeed;
        }
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        ApplyFixedMovement(direction);
    }
}
