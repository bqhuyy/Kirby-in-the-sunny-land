using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayMenu : MonoBehaviour
{
    [SerializeField]
    private SceneFader fader;

    [SerializeField]
    private GameObject replayMenuUI;

    private bool GameIsPause = false;

    const string LevelSelector = "LevelManagement";

    // Update is called once per frame
    void Update()
    {
        if (Kirby.Instance.IsDead && !GameIsPause)
        {
            Pause();
        }
    }

    public void Pause()
    {
        replayMenuUI.SetActive(true);
        Time.timeScale = 0;
        GameIsPause = true;
    }

    public void SelectLevel()
    {
        Time.timeScale = 1;
        fader.FadeTo(LevelSelector);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Replay()
    {
        Time.timeScale = 1;
        fader.FadeTo(SceneManager.GetActiveScene().name);
    }
}
