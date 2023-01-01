using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public GameObject settingPanel;
    public GameObject pausePanel;
    public GameObject creditsPanel;
    public Image howToPlay;

    [SerializeField] GameObject levelPanel;
    [SerializeField] GameObject confirmationPanel;
    [SerializeField] GameObject htplayPanel;
    public List<Sprite> howToPlayeImage = new List<Sprite> { };
    public bool gameIsPaused = false;
    int i = 0;

    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
            howToPlay.sprite = howToPlayeImage[i];
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)&& gameIsPaused == false)
        {
            Pause();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && gameIsPaused == true)
        {
            if (settingPanel.activeSelf == true)
            {
                settingPanel.SetActive(false);
                pausePanel.SetActive(true);
            }
            else
            {
                Resume();
            }            
        }
        
    }
    public void Play()
    {
        levelPanel.SetActive(true);
    }

    public void Setting()
    {
        settingPanel.SetActive(true);
        if(SceneManager.GetActiveScene().buildIndex != 0)
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
        if(SceneManager.GetActiveScene().buildIndex != 0)
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
        Time.timeScale = 1;
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

    public void Next()
    {
        Debug.Log(i);
        if(howToPlay.sprite != howToPlayeImage[4])
        {
            i++;
            howToPlay.sprite = howToPlayeImage[i];
            Debug.Log(i);
        }
    }

    public void CloseHtP()
    {
        htplayPanel.SetActive(false);
    }

    public void HowtoPlay()
    {
        htplayPanel.SetActive(true);
    }

    public void Prev()
    {
        Debug.Log(i);
        if(howToPlay.sprite != howToPlayeImage[0])
        {
            i--;
            howToPlay.sprite = howToPlayeImage[i];
        }
    }
}
