using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//TO DO: Add to Menu Manager
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    [SerializeField] GameObject pauseMenuUI;

    private Input control;

    private AudioManager audioManager;

    void Awake()
    {
        GameIsPaused = false;
        control = new Input();
        control.Enable();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if(!MainMenu.GameStarted)
        {
            return;
        }

        if(control.Player.Pause.triggered)
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }       
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        audioManager.StopAll();
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Quit()
    {
        Time.timeScale = 1f;

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
