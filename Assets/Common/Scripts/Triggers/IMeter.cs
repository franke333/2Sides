using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public abstract class IMeter<T> : SingletonClass<T> where T : MonoBehaviour
{
    // meter variables
    [SerializeField]
    protected float _currentValue;
    [SerializeField]
    protected float _maxValue;
    //<time until alive, multiplication value>
    List<Tuple<float, float>> _multipliers = new List<Tuple<float, float>>();
    protected bool _locked = false;
    [SerializeField]
    protected float _naturalDecayRate = 1f; // per second

    // events
    public UnityEvent<float> ValueUpdated;
    public UnityEvent Triggered;


    //-------public API--------

    public virtual void AddMultiplier(float time, float multiplier)
    {
        _multipliers.Add(new Tuple<float, float>(time + Time.time, multiplier));
    }


    public void Lock() => _locked = true;

    public void Unlock() => _locked = false;

    public virtual void ChangeValue(float value)
    {
        if (_locked)
        {
            return;
        }

        _currentValue = Math.Clamp(_currentValue + value * GetMultiplier(), 0, _maxValue);

        ValueUpdated?.Invoke(_currentValue);

        if (_currentValue >= _maxValue - float.Epsilon)
        {
            OnTrigger();
            Lock();
        }
    }

    // private -------


    protected virtual void Start()
    {
        _currentValue = 0;
    }

    protected virtual void OnTrigger()
    {
        Triggered?.Invoke();
    }




    protected float GetMultiplier()
    {
        if (_multipliers.Count == 0)
        {
            return 1;
        }
        return _multipliers.Select(x => x.Item1).Aggregate((current, next) => current * next);
    }


    protected virtual void Update()
    {
        ChangeValue(-_naturalDecayRate * Time.deltaTime);
        //tick in multipliers
        for (int i = 0; i < _multipliers.Count; i++)
            if (_multipliers[i].Item1 < Time.time)
                _multipliers.RemoveAt(i--);
    }
}
