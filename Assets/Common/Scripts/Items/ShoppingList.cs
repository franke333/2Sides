using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShoppingList : SingletonClass<ShoppingList>
{
    public bool HasAllItems = false;
    private bool hasReachedCheckPoint = false;
    private bool checkCart = false;

    public List<string> items = new List<string>();

    private Dictionary<string, int> wholeList = new Dictionary<string, int>();

    private Dictionary<string, int> cart = new Dictionary<string, int>();

    public Dictionary<string, int> CurrentList = new Dictionary<string, int>();

    private void Start()
    {
        for (int i = 0; i < items.Count; i++)
        {
            wholeList.Add(items[i], Random.Range(1, 4));
        }

        RandomItem(2);

        ShoppingListUI.Instance.UpdateList(CurrentList);
    }
    private bool IsListItem(string itemName)
    {
        if (!CurrentList.ContainsKey(itemName))
            return false;
        //true if there is less in the cart than in the list
        return CurrentList[itemName] > (cart.ContainsKey(itemName) ? cart[itemName] : 0);
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
                RandomItem(4);
                ShoppingListUI.Instance.UpdateList(CurrentList);
                GameManager.Instance.AddTime(20f);
                hasReachedCheckPoint = false;
            }
            else
            {
                HasAllItems = true;
                Debug.Log("Go to cashiers!");
            }
        }
    }

    public void CheckItem(ItemScript itemScript)
    {
        if (!cart.ContainsKey(itemScript.itemName))
        {
            cart.Add(itemScript.itemName, 1);
        }
        else
        {
            cart[itemScript.itemName] += 1;
        }
        GameManager.Instance.AddTime(10f);
        if (CurrentList[itemScript.itemName] == cart[itemScript.itemName])
            ShoppingListUI.Instance.ItemCompleted(itemScript.itemName);
        checkCart = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            ItemScript itemscript = other.GetComponent<ItemScript>();
            if(IsListItem(itemscript.itemName))
            {
                itemscript.StartLockToCart();
            }
            else
            {
                itemscript.StartShootFromCart();
            }
            itemscript.PlaySoundInCart();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            other.GetComponent<ItemScript>().StopCooldown();
        }
    }

    void RandomItem(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var randomItemKey = wholeList.Keys.ElementAt(Random.Range(0, wholeList.Keys.Count - 1));
            var randomItemValue = wholeList[randomItemKey];
            CurrentList.Add(randomItemKey, randomItemValue);
            wholeList.Remove(randomItemKey);
        }
    }
}
