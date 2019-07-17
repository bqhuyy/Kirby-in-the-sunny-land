using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool GameIsPause = false;

    [SerializeField]
    private SceneFader fader;

    [SerializeField]
    private GameObject pauseMenuUI;

    const string StartMenu = "StartMenu";

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Kirby.Instance.IsDead)
        {
            if (GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        GameIsPause = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        fader.FadeTo(StartMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
