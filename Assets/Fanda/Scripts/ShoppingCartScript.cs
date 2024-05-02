using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShoppingCartScript : SingletonClass<ShoppingCartScript>, IInteractable
{
    [SerializeField]
    private GameObject _handleGO;
    private MeshRenderer _handleMR;
    private Color _handleColor;

    private Rigidbody _rb;

    private bool _isBeingPushed;
    [SerializeField]
    private float _allowedDistanceFromPlayer = 10f;

    [SerializeField]
    FixedJoint _joint;

    public float MaxRotationSpeed = 2f;
    public float pushingForce = 200f;

    private void Start()
    {
        _handleMR = _handleGO?.GetComponent<MeshRenderer>();
        _rb = GetComponent<Rigidbody>();
        if(_handleMR != null)
            _handleColor = _handleMR.material.color;
    }

    public void HoverOver()
    {
        if(_handleMR != null)
        {
            _handleMR.material.color = Color.green;
        }
    }

    public void HoverOut()
    {
        if(_handleMR != null)
        {
            _handleMR.material.color = _handleColor;
        }
    }

    private void Update()
    {
        //BeingPushedAdvanced();
    }

    private void BeingPushedSimple(bool value)
    {
        if(value == _isBeingPushed)
        {
            return;
        }
        
        if(value)
        {
            Debug.Log("Shopping cart is being pushed");
            _joint = transform.AddComponent<FixedJoint>();
            _joint.connectedBody = PlayerController.Instance.GetComponent<Rigidbody>();
        }
        else
        {
            Debug.Log("Shopping cart is not being pushed");
            Destroy(_joint);
        }

        _isBeingPushed = value;
    }

    private void BeingPushedAdvanced()
    {
        if (!_isBeingPushed)
            return;

        if(Vector3.Distance(transform.position, PlayerController.Instance.transform.position) > _allowedDistanceFromPlayer)
        {
            _isBeingPushed = false;
            Debug.Log("Shopping cart is not being pushed due to distance");
            return;
        }

        Vector3 towardsTarget = PlayerController.Instance.InFrontOfPlayer.position - transform.position;
        towardsTarget.y = 0;

        //target rotation is to have the player behind the cart
        Quaternion targetRotation = Quaternion.LookRotation(transform.position-PlayerController.Instance.transform.position, Vector3.up);

        _rb.AddTorque(Vector3.up * Mathf.Min(1, towardsTarget.magnitude) * MaxRotationSpeed, ForceMode.Force);
        _rb.AddForce(towardsTarget.normalized * Mathf.Min(1,towardsTarget.magnitude) * pushingForce, ForceMode.Force);
        
        
    }

    public void Interact(bool value)
    {
        BeingPushedSimple(value);
    }
}
