using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TriggerMeter : IMeter<TriggerMeter>
{
    private Volume _volume;
    private Vignette _vignette;
    public AnimationCurve _vignetteCurve;

    protected override void Start()
    {
        base.Start();
        _volume = GetComponent<Volume>();
        if (!_volume.profile.TryGet<Vignette>(out _vignette))
            Debug.LogWarning("Vignette not found");
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeValue(25);
        }
        _vignette.intensity.value = _vignetteCurve.Evaluate(_currentValue / _maxValue);
    }

    protected override void OnTrigger()
    {
        Debug.Log("Triggered");
        ShoppingCartScript.Instance.DoAFlip();
        base.OnTrigger();
        ChangeValue(-_maxValue);
        Invoke("Unlock", 1);
    }
}
