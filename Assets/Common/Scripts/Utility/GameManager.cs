using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : SingletonClass<GameManager>
{
    public bool tutorial = false;

    [SerializeField] float CurrentTime = 60f;


    [SerializeField] TextMeshProUGUI Timer;
    [SerializeField] List<ConfettiScript> tutorialConfettis;
    [SerializeField] GameObject tutorialCongratsText;
    [SerializeField] GameObject PauseMenu;

    SecurityManager _sm;
    bool _gameOver = false;

    float startingTime;

    public bool GameOverSequence => _gameOver;

    void Start()
    {
        Time.timeScale = 1;
        _sm = SecurityManager.Instance;
        startingTime = CurrentTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMenu();
        }
        if (CurrentTime > 0 && !_gameOver)
        {
            CurrentTime -= 1 * Time.deltaTime;
        }
        else
        {
            if (!_gameOver)
                AudioManager.Instance.PlaySecurityAlerted();
            _gameOver = true;
            CurrentTime = 0;
            Timer.color = Color.red;
            _sm.GameOverRunToPlayer();
        }

        int min = Mathf.FloorToInt(CurrentTime / 60);
        int sec = Mathf.FloorToInt(CurrentTime % 60);
        Timer.text = string.Format("{0:00}:{1:00}", min, sec);
    }

    public void AddTime(float amount)
    {
        if (_gameOver)
            return;
        CurrentTime += amount;
        StartCoroutine(ColorTimer(Color.green));
        AudioManager.Instance.PlayTimeAdded();
    }

    public void RemoveTime(float amount)
    {
        if (_gameOver)
            return;
        CurrentTime -= amount;
        StartCoroutine(ColorTimer(Color.red));
        AudioManager.Instance.PlayTimeRemoved();
    }

    public void GameOver()
    {
        SceneManager.LoadScene("LooseScene");
    }

    public void GameWon()
    {
        if(tutorial)
        {
            foreach (var item in tutorialConfettis)
            {
                item.active = true;
            }
            tutorialCongratsText.SetActive(true);
            //after 5 seconds load the next scene
            StartCoroutine(LoadMainMenu());
            return;
        }

        SettingsManager.Instance.EndTime = CurrentTime;

        SceneManager.LoadScene("WinScene");
    }

    IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(8);
        SceneManager.LoadScene("Title Screen");
    }

    public float GetTime() => CurrentTime;

    IEnumerator ColorTimer(Color color)
    {
        Color originalColor = Timer.color;
        Timer.color = color;
        yield return new WaitForSeconds(2);
        Timer.color = originalColor;
    }

    public void OpenMenu()
    {
        PauseMenu.SetActive(true);
        //unlock mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }
}
