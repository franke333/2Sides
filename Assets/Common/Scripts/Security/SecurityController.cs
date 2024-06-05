using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SecurityController : MonoBehaviour
{
    [SerializeField]
    private GameObject _hand1, _hand2;
    [SerializeField]
    private Transform _eye;

    [SerializeField]
    private float _minTurnWaitTime = 1f, _maxTurnWaitTime = 10f;
    [SerializeField]
    private float _turnAngle = 90f;
    [SerializeField]
    private float _turnDuration = 0.6f;

    private float _currentTurnWaitTime = 0f;

    [SerializeField]
    private float _seeRange = 15f;

    private List<Vector3> _playerRayHits = new List<Vector3>(), _itemRayHits = new List<Vector3>();
    private float _gizmosCd = -10f;

    public bool Chasing = false;
    float speedUpdate = 0f;

    Tween _anim;
    NavMeshAgent _navMeshAgent;

    private void Start()
    {
        SecurityManager.Instance.securities.Add(this);
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Turn()
    {
        _currentTurnWaitTime = Random.Range(_minTurnWaitTime, _maxTurnWaitTime);
        transform.DORotate(new Vector3(0, _turnAngle, 0), _turnDuration).SetRelative();
    }

    private void Update()
    {
        //Debug.Log(speedUpdate);
        if (Chasing)
        {
            if (speedUpdate > 5f)
            {
                _navMeshAgent.speed += 1.25f;
                speedUpdate = 0f;
            }
            speedUpdate += Time.deltaTime;
            RunAnimation();
        }
        //currently moving. stop turning
        if(_navMeshAgent.hasPath)
            return;

        if (_currentTurnWaitTime > 0)
            _currentTurnWaitTime -= Time.deltaTime;
        else
            Turn();

        _gizmosCd -= Time.deltaTime;
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
        PlayerController player = PlayerController.Instance;
        if (Vector3.Distance(hit.transform.position, _eye.position) > _seeRange)
            return;
        if(Vector3.Distance(hit.transform.position,player.transform.position) > _seeRange)
            return;

        var sm = SecurityManager.Instance;

        _playerRayHits.Clear();
        _itemRayHits.Clear();
        _gizmosCd = 5f;
        //shoot rays at player
        RaycastHit hitInfo;
        int hits = 0;
        for (int i = 0; i < sm.RaysForPlayer; i++)
        {
            Vector3 point = GetRandomPointInCollider(player.colliders[Random.Range(0, player.colliders.Length)].bounds);
            if (Physics.Raycast(_eye.position, point - _eye.position, out hitInfo, _seeRange))
            {
                //Player layer is 3
                if (hitInfo.collider.gameObject.layer == 3)
                    hits++;
                _playerRayHits.Add(hitInfo.point);
            }
        }
        if(hits < sm.MinimumRaysToHitForPlayer)
        {
            Debug.Log($"Player is not in sight: {hits}/{sm.RaysForPlayer} hits");
            return;
        }

        //shoot item
        hits = 0;
        for (int i = 0; i < sm.RaysForItem; i++)
        {
            Vector3 point = GetRandomPointInCollider(hit.colliders[Random.Range(0, hit.colliders.Length)].bounds);
            if (Physics.Raycast(_eye.position, point - _eye.position, out hitInfo, _seeRange))
            {
                if (hitInfo.collider.gameObject == hit.gameObject)
                    hits++;
                _itemRayHits.Add(hitInfo.point);
            }
        }
        if (hits < sm.MinimumRaysToHitForItem)
        {
            Debug.Log($"Item is not in sight: {hits}/{sm.RaysForItem} hits");
            return;
        }
        hit.ReportSeeingHittingGround();


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


    public void MoveTo(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _seeRange);

        if (_gizmosCd < 0)
            return;

        Gizmos.color = Color.blue;
        foreach (var point in _playerRayHits)
        {
            Gizmos.DrawLine(_eye.position, point);
            Gizmos.DrawSphere(point, 0.1f);
        }
        Gizmos.color = Color.yellow;
        foreach (var point in _itemRayHits)
        {
            Gizmos.DrawLine(_eye.position, point);
            Gizmos.DrawSphere(point, 0.1f);
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if(GameManager.Instance == null)
        {
            return;
        }
        if (GameManager.Instance.GameOverSequence && col.gameObject.layer == 3) // If the collision is with Player layer
        {
            GameManager.Instance.GameOver();
        }
    }
}
