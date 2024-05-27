using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingList : MonoBehaviour
{
    public bool HasAllItems = false; // In the future this will change for hasAllItems or something like that

    private Dictionary<string, int> shoppingList = new Dictionary<string, int>();

    private void Update()
    {
        if (shoppingList.ContainsKey("Milk") && shoppingList.ContainsKey("Apple") && shoppingList.ContainsKey("Bread"))
        {
            if (shoppingList["Milk"] == 2 && shoppingList["Apple"] == 1 && shoppingList["Bread"] == 1)
            {
                HasAllItems = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            if (!shoppingList.ContainsKey(other.GetComponent<ItemScript>().itemName))
            {
                shoppingList.Add(other.GetComponent<ItemScript>().itemName, 1);
            }
            else
            {
                shoppingList[other.GetComponent<ItemScript>().itemName] += 1;
            }

            GameManager.Instance.AddTime(20f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            if (shoppingList[other.GetComponent<ItemScript>().itemName] == 1)
            {
                shoppingList.Remove(other.GetComponent<ItemScript>().itemName);
            }
            else
            {
                shoppingList[other.GetComponent<ItemScript>().itemName] -= 1;
            }
        }
    }
}
