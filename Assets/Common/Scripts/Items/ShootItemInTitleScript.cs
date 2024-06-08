using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootItemInTitleScript : MonoBehaviour
{
    const int probability = 10000;
    private void FixedUpdate()
    {
        if(Random.Range(0, probability) == 0)
        {
            GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * Random.Range(20,200), ForceMode.Impulse);
        }
    }
}
