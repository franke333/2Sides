using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityController : MonoBehaviour
{
    [SerializeField]
    private GameObject _hand1, _hand2;

    [SerializeField]
    private float _minTurnWaitTime = 1f, _maxTurnWaitTime = 10f;
    [SerializeField]
    private float _turnAngle = 90f;
    [SerializeField]
    private float _turnDuration = 0.6f;

    private float _currentTurnWaitTime = 0f;

    [SerializeField]
    private float _seeRange = 15f;

    Tween _anim;

    private void Turn()
    {
        _currentTurnWaitTime = Random.Range(_minTurnWaitTime, _maxTurnWaitTime);
        transform.DORotate(new Vector3(0, _turnAngle, 0), _turnDuration).SetRelative();
    }

    private void Update()
    {
        //RunAnimation();
        if (_currentTurnWaitTime > 0)
            _currentTurnWaitTime -= Time.deltaTime;
        else
            Turn();
    } 

    private void RunAnimation()
    {
        if(_anim != null && _anim.IsActive())
        {
            return;
        }

        var sequence = DOTween.Sequence();
        sequence.Append(_hand1.transform.DOLocalRotate(new Vector3(89,0,0),0.25f));
        sequence.Join(_hand2.transform.DOLocalRotate(new Vector3(-89,0,0),0.25f));
        sequence.Append(_hand1.transform.DOLocalRotate(new Vector3(-89,0,0),.5f));
        sequence.Join(_hand2.transform.DOLocalRotate(new Vector3(89,0,0),.5f));
        sequence.Append(_hand1.transform.DOLocalRotate(new Vector3(0,0,0),0.25f));
        sequence.Join(_hand2.transform.DOLocalRotate(new Vector3(0,0,0),0.25f));
        _anim = sequence;
        _anim.Play();
    }

    public void AlertItemDrop(HittingGroundSusScript hit)
    {

    }

    private Vector3 GetRandomPointInCollider(Bounds colliderBound)
    {
        var point = new Vector3(
            Random.Range(colliderBound.min.x, colliderBound.max.x),
            Random.Range(colliderBound.min.y, colliderBound.max.y),
            Random.Range(colliderBound.min.z, colliderBound.max.z)
            );
        return point;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _seeRange);
    }
}
