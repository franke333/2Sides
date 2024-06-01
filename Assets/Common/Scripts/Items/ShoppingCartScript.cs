using DG.Tweening;
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
    Joint _joint;

    public float MaxRotationSpeed = 2f;
    public float throwForce = 200f;

    private bool _locked = false;

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

    //Oh god this needs to be refactored
    private void Lock()
    {
        _locked = true;
    }

    private void Unlock()
    {
        _locked = false;
    }

    public void DoAFlip()
    {
        if (_locked)
            return;
        _locked = true;
        //deattach player or he will be reaching outer space
        BeingPushedSimple(false);

        Invoke("Unlock", 1f);

        DOTween.Sequence()
            .Append(_rb.DOMoveY(2, 0.5f).SetEase(Ease.OutBounce))
            .Append(_rb.DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360))
            .Append(_rb.DOMoveY(0, 0.5f).SetEase(Ease.OutBounce));
    }

    private void BeingPushedSimple(bool value)
    {
        if(value == _isBeingPushed)
        {
            return;
        }
        
        if(value)
        {
            if (_locked)
                return;
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

    public void InteractView(bool value)
    {

    }

    public void Touch(bool value, Transform h)
    {
        BeingPushedSimple(value);
    }

    public void Throw()
    {
        if (_locked)
            return;
        _locked = true;
        //deattach player or he will be reaching outer space
        BeingPushedSimple(false);

        Invoke("Unlock", 1f);

        _rb.AddForce(PlayerController.Instance.transform.forward * throwForce, ForceMode.Impulse);
    }
}
