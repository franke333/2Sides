using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : PersistentSingletonClass<SettingsManager>
{
    public float SFXVolume = 1f;
    public float MusicVolume = 1f;
    public float MasterVolume = 1f;

    public float MouseSensitivity = 1f;
}
