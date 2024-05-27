using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class ShoppingListUI : SingletonClass<ShoppingListUI>
{
    public TextMeshProUGUI List;

    public List<string> items = new List<string>();

    private int item = 0;

    public void FillList(Dictionary<string, int> list)
    {
        for (int i = item; i < list.Count; i++)
        {
            item = i + 1;
            List.text += list.Values.ElementAt(i) + "x " + list.Keys.ElementAt(i) + "\n";
        }
    }
}
