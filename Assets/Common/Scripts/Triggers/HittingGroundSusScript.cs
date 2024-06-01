using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittingGroundSusScript : MonoBehaviour
{
    public float timeDecrease = 20f;
    public float cooldown = 560f;

    private float _currentCooldown = 0f;

    public Collider[] colliders;



    public void ReportSeeingHittingGround()
    {
        if (_currentCooldown > 0)
            return;

        _currentCooldown = cooldown;
        GameManager.Instance.RemoveTime(timeDecrease);
        Debug.Log("Hitting ground -> decreasing " + timeDecrease + " from time");
        return;
    }

    private void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
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
            //ReportSeeingHittingGround();
            SecurityManager.Instance.securities.ForEach(security => security.AlertItemDrop(this));
        }
    }
}
