using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : PersistentSingletonClass<SettingsManager>
{
    public float SFXVolume = 1f;
    public float MusicVolume = 1f;
    public float MasterVolume = 1f;

    public float MouseSensitivity = 1f;

    public float EndTime;


    public List<string> quotes = new List<string>();
    public List<string> tips = new List<string>();

    public string GetRandomTitleQuote()
    {
        return quotes[Random.Range(0, quotes.Count)];

    }

    public string GetRandomLoseScreenTip()
    {
        return tips[Random.Range(0, tips.Count)];
    }
}
