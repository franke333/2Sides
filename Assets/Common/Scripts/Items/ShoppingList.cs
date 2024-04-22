using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingList : MonoBehaviour
{
    public bool HasMilk; // In the future this will change for hasAllItems or something like that

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Milk")
        {
            HasMilk = true;
            Debug.Log("mam mleko");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Milk")
        {
            HasMilk = false;
        }
    }
}
