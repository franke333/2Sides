using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : SingletonClass<TutorialManager>
{
    public GameObject rightArm;

    public GameObject timer;

    public GameObject gate1, gate2, gate3;

    public Light finalLight;

    int targets = 2;
    int items = 1;

    private void Start()
    {
        rightArm.SetActive(false);
        timer.SetActive(false);
    }

    public void TargetHit()
    {
        targets--;
        if (targets == 0)
        {
            TargetCheckpointDone();
        }
    }

    private void PushDownGate(GameObject gate)
    {
        foreach(var col in gate.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
        gate.transform.DOMoveY(-5f,3f).OnComplete(() => gate.SetActive(false));
    }


    public void TargetCheckpointDone()
    {
        rightArm.SetActive(true);
        ShoppingListUI listUI = ShoppingListUI.Instance;
        ShoppingList list = ShoppingList.Instance;
        PushDownGate(gate1);
        //TODO list
        list.CurrentList.Add("Milk", 1);
        listUI.UpdateList(list.CurrentList);

    }

    public void ItemAdded()
    {
        items--;
        if (items == 0)
        {
            FirstShoppingListDone();
        }
    }

    public void FirstShoppingListDone()
    {
        timer.SetActive(true);
        PushDownGate(gate2);
        ShoppingList list = ShoppingList.Instance;
        list.CurrentList.Add("Banana", 2);
        list.CurrentList.Add("Eggs", 1);
        list.CurrentList.Add("Apple", 3);
        ShoppingListUI.Instance.UpdateList(list.CurrentList);
    }

    IEnumerator ChangeLightColor()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            finalLight.color = Random.ColorHSV(0, 1, 1, 1, 1, 1, 1, 1);
        }
    }

    public void TutorialDone()
    {
        StartCoroutine(ChangeLightColor());
        PushDownGate(gate3);
    }

}

