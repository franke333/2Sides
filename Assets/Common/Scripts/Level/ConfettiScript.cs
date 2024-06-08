using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiScript : MonoBehaviour
{

    public List<ItemScript> prefabs = new List<ItemScript>();

    public float cooldown = 0.1f;
    private float timer = 0f;
    public float force = 10f;

    public Transform shootTowards;

    ItemScript lastItem;

    public bool active = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        timer = cooldown;
        //shoot confetti
        lastItem = Instantiate(prefabs[Random.Range(0, prefabs.Count)], transform.position, Quaternion.identity);
        lastItem.transform.SetParent(transform);
        float newForce = force * Random.Range(0.5f, 1.5f);
        lastItem.GetComponent<Rigidbody>().AddForce((shootTowards.position - transform.position).normalized * newForce, ForceMode.Impulse);
        Destroy(lastItem);


    }
}
