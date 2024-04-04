using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    Material _mat;
    public float speed;
    public float colorSpeed;
    private float _time;
    private float _colorTime;

    // Start is called before the first frame update
    void Start()
    {
       _mat = GetComponent<Renderer>().material;
        _time = 0;

    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime * speed;
        _colorTime += Time.deltaTime * colorSpeed;
        
        float x = Mathf.Sin(_time) * 0.5f + 0.5f;
        float z = Mathf.Cos(_time) * 0.5f + 0.5f;
        transform.position = new Vector3(x, 0, z);

        float r = Mathf.Sin(_colorTime) * 0.5f + 0.5f;
        float g = Mathf.Cos(_colorTime) * 0.5f + 0.5f;
        float b = Mathf.Sin(_colorTime*2) * 0.5f + 0.5f;

        _mat.color = new Color(r, g, b);
    }
}
