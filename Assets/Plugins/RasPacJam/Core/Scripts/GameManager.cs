using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RasPacJam.Audio;

public class GameManager : MonoBehaviour
{
    public bool IsGameRunning => isGameRunning;

    [SerializeField] private UIManager uiManager = null;
    [SerializeField] private SceneLoader sceneLoader = null;
    private bool isGameRunning;



    public void PauseGame()
    {
        isGameRunning = false;
        Time.timeScale = 0;
        uiManager.OpenPauseWindow();
    }

    public void ResumeGame()
    {
        isGameRunning = true;
        Time.timeScale = 1;
        uiManager.ClosePauseWindow();
    }

    public void ResetGame()
    {
        Time.timeScale = 1;
        sceneLoader.LoadLevel(0);
    }

    public void EndGame()
    {
        uiManager.OpenGameOverWindow();
    }

    public void QuitGame()
    {
        Application.Quit();
    }



    private void Start()
    {
        InitializeGame();
        AudioManager.Instance.ResetMusic();
    }

    private void InitializeGame()
    {
        Time.timeScale = 1;
        isGameRunning = true;
    }
}
