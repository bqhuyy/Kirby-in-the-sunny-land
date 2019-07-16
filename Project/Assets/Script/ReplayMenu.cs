using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject replayMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        Kirby.Instance.Dead += new DeadEventHandler(Pause);
    }

    private void Update()
    {
        if (Kirby.Instance.isWin)
        {
            replayMenuUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()
    {
        StartCoroutine(ShowReplay());
    }

    IEnumerator ShowReplay()
    {
        yield return new WaitForSeconds(1.0f);
        replayMenuUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void SelectLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelManagement");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
