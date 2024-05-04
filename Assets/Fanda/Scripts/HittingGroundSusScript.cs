using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittingGroundSusScript : MonoBehaviour
{
    public float susIncrease = 20f;
    public float cooldown = 60f;

    private float _currentCooldown = 0f;

    public BoxCollider[] colliders;



    public void ReportSeeingHittingGround()
    {
        if (_currentCooldown > 0)
            return;

        _currentCooldown = cooldown;
        SusMeter.Instance.ChangeValue(susIncrease);
        Debug.Log("Hitting ground -> adding " + susIncrease + " to sus");
        return;
    }

    private void Start()
    {
        colliders = GetComponentsInChildren<BoxCollider>();
    }

    private void Update()
    {
        if (_currentCooldown > 0)
            _currentCooldown -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //TODO alert security
            ReportSeeingHittingGround();
        }
    }
}
