using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ItemScript : MonoBehaviour, IInteractable
{
    public string itemName;
    public int throwForce;

    private PlayerCameraScript _playerCameraScript;

    MeshRenderer[] _meshRenderers;

    [SerializeField]
    AudioClip _fallToTheGroundClip;
    [SerializeField]
    AudioClip _putInCartClip;
    AudioSource _source;
    Rigidbody _rb;

    float _lastVelocity = 0.5f;

    bool _cooldownRunning = false;
    [SerializeField]
    float _baceCooldown = 1f;
    float _cooldown;
    bool _toBeLocked = false; //false means it will be shot from cart and decrease Time


    private void Start()
    {
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();

        _playerCameraScript = FindObjectOfType<PlayerCameraScript>();

        gameObject.AddComponent<HittingGroundSusScript>();
        _source = gameObject.AddComponent<AudioSource>();
        _source.spatialBlend = AudioManager.Instance._3dSpatialBlendForItemSFX;
        _rb = GetComponent<Rigidbody>();

        _cooldown = _baceCooldown;
    }

    private void FixedUpdate()
    {
        _lastVelocity = _rb.velocity.magnitude;
    }

    public void Update()
    {
        if (!_cooldownRunning)
            return;
        _cooldown -= Time.deltaTime;
        if (_cooldown < 0)
            OnCooldownFInish();
    }

    public void HoverOver()
    {
    }

    public void HoverOut()
    {
    }

    public void InteractView(bool value)
    {
        //Do nothing
    }

    public void PlaySoundOnGround()
    {
        if (_source.isPlaying)
            return;
        float velocity = Mathf.Max(_lastVelocity, _rb.velocity.magnitude);
        _source.volume = Mathf.Clamp(velocity/ AudioManager.Instance._minimalVelocityForPlayingHitSound, 0, 1);
        _source.clip = _fallToTheGroundClip;
        _source.Play();
    }

    public void PlaySoundInCart()
    {
        if (_source.isPlaying)
            return;
        _source.volume = 1;
        _source.clip = _putInCartClip;
        _source.Play();
    }

    public void Touch(bool value, Transform h)
    {
        if (value)
        {
            foreach (var collider in GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(PlayerController.Instance.Body.GetComponent<Collider>(), collider, true);
                Physics.IgnoreCollision(PlayerController.Instance.Head.GetComponent<Collider>(), collider, true);
            }
            transform.GetComponent<Rigidbody>().isKinematic = true;
            transform.parent = h.transform;
            transform.position = h.transform.position;
            return;
        }
        else
        {
            transform.parent = null;
            transform.GetComponent<Rigidbody>().isKinematic = false;
            foreach (var collider in GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(PlayerController.Instance.Body.GetComponent<Collider>(), collider, true);
                Physics.IgnoreCollision(PlayerController.Instance.Head.GetComponent<Collider>(), collider, true);
            }
        }
    }

    public void Throw()
    {
        StartCoroutine(Untouchable());
        transform.parent = null;
        transform.GetComponent<Rigidbody>().isKinematic = false;
        transform.GetComponent<Rigidbody>().AddForce(_playerCameraScript.GetViewVector() * throwForce);
        AudioManager.Instance.PlayThrowItem();
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            Physics.IgnoreCollision(PlayerController.Instance.Body.GetComponent<Collider>(), collider, true);
            Physics.IgnoreCollision(PlayerController.Instance.Head.GetComponent<Collider>(), collider, true);
        }
    }

    private IEnumerator Untouchable()
    {
        float countDown = 1f;
        while (countDown >= 0)
        {
            Physics.IgnoreLayerCollision(3, 6, true); // Ignore collisions for item and player so it could fly away
            countDown -= Time.deltaTime;
            yield return null;
        }
        Physics.IgnoreLayerCollision(3, 6, false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlaySoundOnGround();
        if (collision.gameObject.tag == "Ground")
        {
            transform.gameObject.layer = 10;

            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                _meshRenderers[i].material.color = Color.black;
            }
        }
    }



    #region cart interaction

    private void OnCooldownFInish()
    {
        if (_toBeLocked)
        {
            gameObject.tag = "Untagged";
            ShoppingList.Instance.CheckItem(this);
            transform.SetParent(ShoppingCartScript.Instance.transform);
            ShoppingCartScript.Instance.GetComponent<Rigidbody>().mass += _rb.mass;
            Destroy(_rb);
            Destroy(this); //this script
        }
        else
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 direction = 2*Vector3.up + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
            _rb.AddForce(direction * 5, ForceMode.Impulse);
            _cooldownRunning = false;
            _cooldown = _baceCooldown;
        }
    }

    public void StartLockToCart()
    {
        Debug.Log("StartLockToCart");
        _toBeLocked = true;
        _cooldown = _baceCooldown;
        _cooldownRunning = true;
    }

    public void StartShootFromCart()
    {
        Debug.Log("StartShootFromCart");
        _toBeLocked = false;
        _cooldown = _baceCooldown;
        _cooldownRunning = true;
    }

    public void StopCooldown()
    {
        Debug.Log("stopCooldown");
        _cooldownRunning = false;
        _cooldown = _baceCooldown;
    }

    #endregion
}