using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject menu;

    PlayerInput action;

    private void Awake()
    {
        action = new PlayerInput();
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        paused = true;

        menu.SetActive(true);

        // When Audio will be on, volume down?
        // AudioListener.pause = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        paused = false;
        menu.SetActive(false);

        // When Audio will be on, volume down?
        // AudioListener.pause = false;
    }

    private void Start()
    {
        action.InputControls.PauseGame.performed += ctx => DeterminePause();
    }

    private void DeterminePause()
    {
        if (paused) ResumeGame();
        else PauseGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    private void OnEnable()
    {
        action.InputControls.PauseGame.Enable();
    }

    private void OnDisable()
    {
        action.InputControls.PauseGame.Disable();
    }


}