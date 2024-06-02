using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShoppingList : MonoBehaviour
{
    public bool HasAllItems = false;
    private bool hasReachedCheckPoint = false;
    private bool checkCart = false;

    private Dictionary<string, int> wholeList = new Dictionary<string, int>();

    private Dictionary<string, int> cart = new Dictionary<string, int>();

    public Dictionary<string, int> CurrentList = new Dictionary<string, int>();

    private void Start()
    {
        for (int i = 0; i < ShoppingListUI.Instance.items.Count; i++)
        {
            wholeList.Add(ShoppingListUI.Instance.items[i], Random.Range(1, 3));
        }

        RandomItem(2);

        ShoppingListUI.Instance.FillList(CurrentList);
    }

    private void Update()
    {
        if (checkCart)
        {
            for (int i = 0; i < CurrentList.Count; i++)
            {
                var item = CurrentList.Keys.ElementAt(i);
                if (cart.ContainsKey(item))
                {
                    if (cart[item] == CurrentList.Values.ElementAt(i))
                    {
                        hasReachedCheckPoint = true;
                    }
                    else
                    {
                        hasReachedCheckPoint = false;
                        break;
                    }
                }
                else
                {
                    hasReachedCheckPoint = false;
                    break;
                }
            }
            checkCart = false;
        }

        if (hasReachedCheckPoint)
        {
            if (CurrentList.Count < 3)
            {
                RandomItem(5);
                ShoppingListUI.Instance.FillList(CurrentList);
                GameManager.Instance.AddTime(20f);
                hasReachedCheckPoint = false;
            }
            else
            {
                HasAllItems = true;
                Debug.Log("WON!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            ItemScript itemScript = other.GetComponent<ItemScript>();
            if (!cart.ContainsKey(itemScript.itemName))
            {
                cart.Add(itemScript.itemName, 1);
            }
            else
            {
                cart[itemScript.itemName] += 1;
            }
            itemScript.PlaySoundInCart();
            checkCart = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            if (cart[other.GetComponent<ItemScript>().itemName] == 1)
            {
                cart.Remove(other.GetComponent<ItemScript>().itemName);
            }
            else
            {
                cart[other.GetComponent<ItemScript>().itemName] -= 1;
            }

            checkCart = true;
        }
    }

    void RandomItem(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var randomItemKey = wholeList.Keys.ElementAt(Random.Range(0, wholeList.Keys.Count - 1));
            var randomItemValue = wholeList.Values.ElementAt(Random.Range(0, wholeList.Values.Count - 1));
            CurrentList.Add(randomItemKey, randomItemValue);
            wholeList.Remove(randomItemKey);
        }
    }
}
