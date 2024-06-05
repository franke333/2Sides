using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ShoppingList : SingletonClass<ShoppingList>
{
    public bool HasAllItems = false;
    private bool hasReachedCheckPoint = false;
    private bool checkCart = false;

    private List<GameObject> objectsInCart = new List<GameObject>();

    public List<string> items = new List<string>();

    private Dictionary<string, int> wholeList = new Dictionary<string, int>();

    private Dictionary<string, int> cart = new Dictionary<string, int>();

    public Dictionary<string, int> CurrentList = new Dictionary<string, int>();

    private void Start()
    {
        if (GameManager.Instance == null || GameManager.Instance.tutorial)
        {
            return;
        }
        for (int i = 0; i < items.Count; i++)
        {
            int count = Random.Range(1, 4);
            if (items[i] == "Casserole" || items[i] == "Watermelon")
                count = 1;
            wholeList.Add(items[i], count);
        }

        RandomItem(2);

        ShoppingListUI.Instance.UpdateList(CurrentList);
    }
    private bool IsListItem(string itemName)
    {
        if (GameManager.Instance.tutorial && itemName == "Tomato")
            return true;
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

        if (hasReachedCheckPoint && !HasAllItems)
        {
            if (GameManager.Instance.tutorial)
            {
                Debug.Log("Tutorial check");
                if (CurrentList.Count > 1)
                {
                    Debug.Log("Tutorial complete");
                    HasAllItems = true;
                    TutorialManager.Instance.TutorialDone();
                }
                else
                {
                    Debug.Log("Tutorial next");
                    hasReachedCheckPoint = false;
                    TutorialManager.Instance.FirstShoppingListDone();
                }
                return;
            }
            if (CurrentList.Count < 3)
            {
                RandomItem(4);
                ShoppingListUI.Instance.UpdateList(CurrentList);
                GameManager.Instance.AddTime(25f);
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
        if(GameManager.Instance.tutorial && itemScript.itemName == "Tomato")
            return;
        if (!cart.ContainsKey(itemScript.itemName))
        {
            cart.Add(itemScript.itemName, 1);
        }
        else
        {
            cart[itemScript.itemName] += 1;
        }
        if (CurrentList[itemScript.itemName] == cart[itemScript.itemName])
        {
            ShoppingListUI.Instance.ItemCompleted(itemScript.itemName);
            GameManager.Instance.AddTime(10f + 2 * CurrentList[itemScript.itemName]);
        }
        objectsInCart.Add(itemScript.gameObject);
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

    public void RemoveAllItemsFromCart()
    {
        //TODO: Remove all items from cart
        cart.Clear();
        HasAllItems = false;
        hasReachedCheckPoint = false;
        ShoppingListUI.Instance.ResetList();
        foreach (var item in objectsInCart)
        {
            foreach (var mr in item.GetComponentsInChildren<MeshRenderer>())
                mr.material.color = Color.black;
            StartCoroutine(ShootItem(item));
        }
        objectsInCart.Clear();
    }

    IEnumerator ShootItem(GameObject item)
    {
        yield return new WaitForSeconds(Random.Range(0.1f,1f));
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 direction = 2 * transform.up + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
        var rb = item.AddComponent<Rigidbody>();
        item.transform.SetParent(null);
        rb.isKinematic = false;
        rb.mass = 1;
        rb.AddForce(direction * Random.Range(5,15), ForceMode.Impulse);
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
