using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShoppingListUI : SingletonClass<ShoppingListUI>
{
    public TextMeshProUGUI List;

    private List<string> items = new List<string>();

    [SerializeField]
    List<Sprite> spritesForQuantities = new List<Sprite>();

    [SerializeField]
    List<Image> imagesItems = new List<Image>();
    [SerializeField]
    List<Image> imagesQuantity = new List<Image>();

    private Dictionary<string, Sprite> textMap;

    protected override void Awake()
    {
        base.Awake();
        var resources = Resources.LoadAll<Sprite>("ShoppingList");
        textMap = new Dictionary<string, Sprite>();
        foreach (var item in resources)
        {
            textMap.Add(item.name, item);
        }
        foreach (var item in imagesItems)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in imagesQuantity)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void ItemCompleted(string itemName)
    {
        int index = items.IndexOf(itemName);
        imagesItems[index].sprite = textMap[itemName.ToLower() + "_ST"];
    }

    public void UpdateList(Dictionary<string, int> list)
    {
        foreach(var (item,count) in list)
        {
            if(items.Contains(item))
                continue;
            items.Add(item);
            Debug.Log("Image items " + imagesItems.Count);
            Debug.Log("TextMap " + textMap.Count);
            imagesItems[items.Count - 1].sprite = textMap[item.ToLower()];
            imagesQuantity[items.Count - 1].sprite = spritesForQuantities[count-1];
            imagesItems[items.Count - 1].gameObject.SetActive(true);
            imagesQuantity[items.Count - 1].gameObject.SetActive(true);
        }
    }
}
