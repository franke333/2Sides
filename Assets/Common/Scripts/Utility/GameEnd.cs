using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            if (sl.HasAllItems)
            {
                Indicator.GetComponent<Renderer>().material.color = Color.green;
                SceneManager.LoadScene("WinScene");
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
