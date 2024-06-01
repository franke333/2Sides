using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyQuest : MonoBehaviour
{
    private float deathFadeTime = 0.6f;
    private MeshRenderer _mr;

    private void Start()
    {
        _mr = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.parent.name == "Karen")
        {
            GameManager.Instance.AddTime(50f);

            col.transform.parent.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
