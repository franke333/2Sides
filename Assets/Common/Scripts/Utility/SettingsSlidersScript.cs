using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSlidersScript : MonoBehaviour
{
    public Slider SFXSlider;
    public Slider MusicSlider;
    public Slider MasterSlider;

    public Slider MouseSlider;

    // Start is called before the first frame update
    void Start()
    {
        var settings = SettingsManager.Instance;

        SFXSlider.value = settings.SFXVolume;
        MusicSlider.value = settings.MusicVolume;
        MasterSlider.value = settings.MasterVolume;
        MouseSlider.value = settings.MouseSensitivity;

        SFXSlider.onValueChanged.AddListener((float value) => settings.SFXVolume = value);
        MusicSlider.onValueChanged.AddListener((float value) => settings.MusicVolume = value);
        MasterSlider.onValueChanged.AddListener((float value) => settings.MasterVolume = value);
        MouseSlider.onValueChanged.AddListener((float value) => settings.MouseSensitivity = value);
    }
}