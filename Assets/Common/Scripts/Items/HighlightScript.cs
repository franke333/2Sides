using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightScript : MonoBehaviour
{
    MeshRenderer[] _meshRenderers;

    Color[] _colors;

    public void Init(MeshRenderer[] meshRenderers, Color highlightColor)
    {
        _meshRenderers = meshRenderers;
        _colors = new Color[_meshRenderers.Length];
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            _colors[i] = _meshRenderers[i].material.color;
        }
    }

    public void Highlight()
    {
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            _meshRenderers[i].material.color = Color.yellow;
        }
    }

    public void RemoveHighlight()
    {
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            _meshRenderers[i].material.color = _colors[i];
        }
    }

}
