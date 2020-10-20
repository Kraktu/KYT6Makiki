using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool IsGameRunning => isGameRunning;
    public float Score { get => score; set => score = Mathf.Max(0, value); }

    [SerializeField] private UIManager uiManager = null;
    [SerializeField] private SceneLoader sceneLoader = null;
    private bool isGameRunning;
    private float score;



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
        uiManager.OpenGameOverWindow(score);
    }

    public void QuitGame()
    {
        Application.Quit();
    }



    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        Time.timeScale = 1;
        isGameRunning = true;
    }
}
