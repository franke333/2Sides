using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyQuest : SingletonClass<BabyQuest>
{
    private float deathFadeTime = 0.6f;
    private MeshRenderer _mr;

    public bool isComplete => gameObject.activeSelf;

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
