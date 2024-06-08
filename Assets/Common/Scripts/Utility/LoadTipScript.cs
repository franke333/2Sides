using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadTipScript : MonoBehaviour
{
    public TMP_Text tipText;
    public bool includeQuotes = false;
    // Start is called before the first frame update
    void Start()
    {
        tipText.text = includeQuotes ? SettingsManager.Instance.GetRandomTitleQuote() : SettingsManager.Instance.GetRandomLoseScreenTip();
    }
}
