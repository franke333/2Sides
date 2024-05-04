using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SusMeter : IMeter<SusMeter>
{

    protected override void OnTrigger()
    {
        base.OnTrigger();
        Debug.Log("Sus meter triggered");
        SecurityManager.Instance.securities.ForEach(security => security.Chasing = true);
    }
}
