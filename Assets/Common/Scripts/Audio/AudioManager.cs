using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public AudioClip lostChildAnnouncment;

    private AudioSource _timeAddedSource;
    private AudioSource _timeRemovedSource;
    private AudioSource _shoppingListUpdatedSource;
    private AudioSource _throwItemSFXSource;
    private AudioSource _securityAlertedSFXSource;
    private AudioSource _lostChildAnnouncmentSource;

    [Header("GLobal Settings")]
    public float _minimalVelocityForPlayingHitSound = 0.5f;
    public float _3dSpatialBlendForItemSFX = 1f;


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

        _lostChildAnnouncmentSource = gameObject.AddComponent<AudioSource>();
        _lostChildAnnouncmentSource.clip = lostChildAnnouncment;

        StartCoroutine(PlayLostChildAndScheduleNewOne());
    }

    private IEnumerator PlayLostChildAndScheduleNewOne()
    {
        yield return new WaitForSeconds(Random.Range(5, 20));
        while (!BabyQuest.Instance.isComplete)
        {
            _lostChildAnnouncmentSource.Play();
            yield return new WaitForSeconds(Random.Range(45, 60));
        }
    }

    private void Update()
    {
        UpdateEscalatingAmbient();
        if(Input.GetKeyDown(KeyCode.M))
        {
            if(_permaAmbientSource.isPlaying)
            {
                _permaAmbientSource.Pause();
            }
            else
            {
                _permaAmbientSource.Play();
            }
        }
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
