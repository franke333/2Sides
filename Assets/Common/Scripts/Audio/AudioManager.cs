using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonClass<AudioManager>
{
    [Header("Ambient")]
    public AudioClip permaAmbient;
    public AudioClip[] escalatingAmbients;
    public float[] escalatingAmbientTimes;

    private AudioSource _permaAmbientSource;
    private AudioSource _escalatingAmbientSource;

    [Header("SFX")]
    public AudioClip timeAdded;
    public AudioClip timeRemoved;
    public AudioClip shoppingListUpdated;
    public AudioClip throwItemSFX;
    public AudioClip securityAlertedSFX;

    private AudioSource _timeAddedSource;
    private AudioSource _timeRemovedSource;
    private AudioSource _shoppingListUpdatedSource;
    private AudioSource _throwItemSFXSource;
    private AudioSource _securityAlertedSFXSource;


    private void Start()
    {
        _permaAmbientSource = gameObject.AddComponent<AudioSource>();
        _permaAmbientSource.clip = permaAmbient;
        _permaAmbientSource.loop = true;
        _permaAmbientSource.Play();

        _escalatingAmbientSource = gameObject.AddComponent<AudioSource>();
        _escalatingAmbientSource.loop = true;
        if (escalatingAmbients.Length > 0)
        {
            _escalatingAmbientSource.clip = escalatingAmbients[0];
            _escalatingAmbientSource.Play();
        }

        _timeAddedSource = gameObject.AddComponent<AudioSource>();
        _timeAddedSource.clip = timeAdded;

        _timeRemovedSource = gameObject.AddComponent<AudioSource>();
        _timeRemovedSource.clip = timeRemoved;

        _shoppingListUpdatedSource = gameObject.AddComponent<AudioSource>();
        _shoppingListUpdatedSource.clip = shoppingListUpdated;

        _throwItemSFXSource = gameObject.AddComponent<AudioSource>();
        _throwItemSFXSource.clip = throwItemSFX;

        _securityAlertedSFXSource = gameObject.AddComponent<AudioSource>();
        _securityAlertedSFXSource.clip = securityAlertedSFX;
    }

    private void Update()
    {
        UpdateEscalatingAmbient();
    }

    private void UpdateEscalatingAmbient()
    {
        if(escalatingAmbients.Length == 0 || escalatingAmbientTimes.Length == 0)
        {
            return;
        }

        AudioClip newClip = null;
        for (int i = 0; i < escalatingAmbientTimes.Length; i++)
        {
            if (GameManager.Instance.GetTime() > escalatingAmbientTimes[i])
            {
                newClip = escalatingAmbients[i];
            }
        }
        newClip ??= escalatingAmbients[escalatingAmbients.Length - 1];
        if(newClip == _escalatingAmbientSource.clip)
        {
            return;
        }
        float time = _escalatingAmbientSource.time;
        //swap
        _escalatingAmbientSource.Stop();
        _escalatingAmbientSource.clip = newClip;
        _escalatingAmbientSource.time = time;
        _escalatingAmbientSource.Play();
    }

    public void PlayThrowItem()
    {
        _throwItemSFXSource.Play();
    }

    public void PlayTimeAdded()
    {
        _timeAddedSource.Play();
    }

    public void PlayTimeRemoved()
    {
        _timeRemovedSource.Play();
    }

    public void PlayShoppingListUpdated()
    {
        _shoppingListUpdatedSource.Play();
    }

    public void PlaySecurityAlerted()
    {
        _securityAlertedSFXSource.Play();
    }

}
