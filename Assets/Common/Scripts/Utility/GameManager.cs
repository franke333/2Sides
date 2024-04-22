using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] float currentTime;

    [SerializeField] TextMeshProUGUI timer;

    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= 1 * Time.deltaTime;
            //print(currentTime);
        }
        else
        {
            currentTime = 0;
            timer.color = Color.red;
            print("GAME OVER");
        }

        int min = Mathf.FloorToInt(currentTime / 60);
        int sec = Mathf.FloorToInt(currentTime % 60);
        timer.text = string.Format("{0:00}:{1:00}", min, sec);
    }
}
