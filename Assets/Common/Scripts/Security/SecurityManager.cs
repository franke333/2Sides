using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SecurityManager : SingletonClass<SecurityManager>
{
    public List<SecurityController> securities;
    public List<Vector3> patrolPoints;

    // this can be done differently
    int _currentSecurityIndex = 0;
    float _untilNextPatrol = 0f;

    private void Start()
    {
        patrolPoints = GameObject.FindGameObjectsWithTag("PatrolPoint").Select(x => x.transform.position).ToList();
        securities = GameObject.FindGameObjectsWithTag("Security").Select(x => x.GetComponent<SecurityController>()).ToList();
        for (int i = 0; i < securities.Count; i++)
        {
            if (securities[i] == null)
            {
                securities.RemoveAt(i);
                i--;
            }
        }
    }

    private void Update()
    {
        if (_untilNextPatrol > 0)
            _untilNextPatrol -= Time.deltaTime;
        else
            SetNewPatrol();
    }

    void SetNewPatrol()
    {
        _currentSecurityIndex = (_currentSecurityIndex + 1) % securities.Count;
        _untilNextPatrol = Random.Range(0.2f, 5f);
        securities[_currentSecurityIndex].MoveTo(patrolPoints[Random.Range(0,patrolPoints.Count)]);
    }



    public int RaysForPlayer = 5;
    public int MinimumRaysToHitForPlayer = 3;
    public int RaysForItem = 5;
    public int MinimumRaysToHitForItem = 3;
}
