using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public SceneFader fader;
    private string levelSelector = "LevelManagement";
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        fader.FadeTo(levelSelector);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
