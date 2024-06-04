using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : SingletonClass<SceneController>
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Play(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Title Screen");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
