using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMeterSlider : MonoBehaviour
{
    Slider _slider;

    Tween _tween;

    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void UpdateMeter(float value)
    {
        //get active tween and kill it
        

        _slider.DOValue(value, 0.15f).SetEase(Ease.InExpo);
    }
}
