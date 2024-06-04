using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyQuest : SingletonClass<BabyQuest>
{
    public bool isComplete => gameObject.activeSelf;

    private void OnTriggerEnter(Collider col)
    {
        if(col.transform.parent == null)
        {
            return;
        }
        if (col.transform.parent.name == "Karen")
        {
            GameManager.Instance.AddTime(50f);

            col.transform.parent.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
