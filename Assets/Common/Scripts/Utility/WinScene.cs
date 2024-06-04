using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScene : SingletonClass<WinScene>
{
    public float EndTime = 0f;

    public TMPro.TextMeshProUGUI Text;

    void Update()
    {
        int min = Mathf.FloorToInt(EndTime / 60);
        int sec = Mathf.FloorToInt(EndTime % 60);
        Text.text = string.Format("The time you spent this run: " + "{0:00}:{1:00}", min, sec);
    }
}
