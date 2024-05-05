using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinOrLoose : SingletonClass<WinOrLoose>
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("CommonScenev0_6");
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}
