using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPrekazkaScript : MonoBehaviour
{
    bool isStatic = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cart")
        {
            if (!isStatic)
            {
                isStatic = true;
                foreach (Transform child in transform)
                {
                    child.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
        }
    }
}
