using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : SingletonClass<GameManager>
{ 

    [SerializeField] float CurrentTime = 60f;

    [SerializeField] TextMeshProUGUI Timer;

    // Update is called once per frame
    void Update()
    {
        if (CurrentTime > 0)
        {
            CurrentTime -= 1 * Time.deltaTime;
        }
        else
        {
            CurrentTime = 0;
            Timer.color = Color.red;
            SceneManager.LoadScene("LooseScene");
        }

        int min = Mathf.FloorToInt(CurrentTime / 60);
        int sec = Mathf.FloorToInt(CurrentTime % 60);
        Timer.text = string.Format("{0:00}:{1:00}", min, sec);
    }

    public void AddTime(float amount)
    {
        CurrentTime += amount;
    }

    public void RemoveTime(float amount)
    {
        CurrentTime -= amount;
    }
}
