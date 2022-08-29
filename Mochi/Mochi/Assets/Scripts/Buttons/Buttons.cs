using UnityEngine.SceneManagement;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public GameObject settingPanel;
    public GameObject pausePanel;
    public GameObject creditsPanel;

    [SerializeField] GameObject levelPanel;
    [SerializeField] GameObject confirmationPanel;
    public bool gameIsPaused;

    public void Play()
    {
        levelPanel.SetActive(true);
    }

    public void Setting()
    {
        settingPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        gameIsPaused = false;
        Time.timeScale = 1;
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        gameIsPaused = true;
        Time.timeScale = 0;
    }
     
    public void CloseSetting()
    {
        settingPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
    
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void Credits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
       confirmationPanel.SetActive(true);
    }

    public void Yes()
    {
        Application.Quit();
    }

    public void No()
    {
        confirmationPanel.SetActive(false);
    }

    public void LevelOne()
    {
        SceneManager.LoadScene(1);
    }
    public void LevelTwo()
    {
        SceneManager.LoadScene(2);
    }
    public void LevelThree()
    {
        SceneManager.LoadScene(3);
    }
    public void LevelBack()
    {
        levelPanel.SetActive(false);
    }
}
