using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    public GameObject[] Indicator;
    Color defaultColor = Color.black;
    public ShoppingList sl;

    private void Start()
    {
        ChangeIndicatoColors(defaultColor);
        sl = ShoppingList.Instance;
    }

    private void ChangeIndicatoColors(Color color)
    {
        foreach (var item in Indicator)
        {
            item.GetComponent<Renderer>().material.color = color;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Cart")
        {
            if (sl.HasAllItems)
            {
                ChangeIndicatoColors(Color.green);
                GameManager.Instance.GameWon();
            }
            else ChangeIndicatoColors(Color.red);
        }
        else
        {
            ChangeIndicatoColors(Color.red);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ChangeIndicatoColors(defaultColor);
    }
}
