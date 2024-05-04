using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMeter : IMeter<TriggerMeter>
{

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeValue(25);
        }
    }

    protected override void OnTrigger()
    {
        Debug.Log("Triggered");
        PlayerController.Instance.DEBUG_CART.GetComponent<ShoppingCartScript>().DoAFlip();
        base.OnTrigger();
        ChangeValue(-_maxValue);
        Invoke("Unlock", 1);
    }
}
