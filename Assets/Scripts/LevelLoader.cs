using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadLevel(string sceneName)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}