using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    public GameObject Indicator;
    Color defaultColor = Color.black;
    public ShoppingList sl;

    private void Start()
    {
        Indicator.GetComponent<Renderer>().material.color = defaultColor;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Cart")
        {
            if (sl.HasMilk)
            {
                Indicator.GetComponent<Renderer>().material.color = Color.green;
                Debug.Log("WIN!");
            }
            else Indicator.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            Indicator.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Indicator.GetComponent <Renderer>().material.color = defaultColor;
    }
}
