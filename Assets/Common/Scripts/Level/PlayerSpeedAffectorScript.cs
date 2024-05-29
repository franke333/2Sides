using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedAffectorScript : MonoBehaviour
{
    public string modifierName;
    public float modifierValue;

    private void OnTriggerStay(Collider other)
    {
        //layer 3 is player
        if (other.gameObject.layer == 3)
        {
            PlayerController.Instance.ApplySpeedModifier(modifierName, modifierValue);
            Debug.Log("Player entered speed affector");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.layer == 3)
        {
            PlayerController.Instance.RemoveSpeedModifier(modifierName);
        }
    }
}
