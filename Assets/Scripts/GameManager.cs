using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject quitBtn;

    public void Start()
    {
        string level = SceneManager.GetActiveScene().name;
        if (level != "Start" && level != "End")
        {
            Resume();
        }

#if UNITY_WEBGL
        quitBtn.gameObject.SetActive(false);
#endif
    }
    private void Update()
    {
        string level = SceneManager.GetActiveScene().name;
        if (level != "Start" && level != "End")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (PublicVars.paused)
                {
                    pauseMenu.SetActive(false);
                    PublicVars.paused = false;
                    Time.timeScale = 1;
                }
                else
                {
                    pauseMenu.SetActive(true);
                    PublicVars.paused = true;
                    Time.timeScale = 0;
                }
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        PublicVars.paused = false;
        Time.timeScale = 1;
    }

    public void Restart()
    {
        string level = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameStart()
    {
        SceneManager.LoadScene("Main");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
