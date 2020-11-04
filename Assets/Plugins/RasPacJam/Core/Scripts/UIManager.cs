using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup pauseWindow = null;
    [SerializeField] private CanvasGroup gameOverWindow = null;
    [SerializeField] private float fadeDuration = 1f;



    public void OpenPauseWindow()
    {
        OpenWindow(pauseWindow);
    }

    public void ClosePauseWindow()
    {
        CloseWindow(pauseWindow);
    }

    public void OpenGameOverWindow()
    {
        OpenWindow(gameOverWindow);
    }

    public void CloseGameOverWindow()
    {
        CloseWindow(gameOverWindow);
    }

    public void OpenWindow(CanvasGroup window)
    {
        window.gameObject.SetActive(true);
        window
                .DOFade(1, fadeDuration)
                .SetEase(Ease.OutQuint)
                .SetUpdate(true);
    }

    public void CloseWindow(CanvasGroup window)
    {
        window
                .DOFade(0, fadeDuration)
                .OnComplete(() => window.gameObject.SetActive(false));
    }



    private void Awake()
    {
        pauseWindow.alpha = 0f;
        pauseWindow.gameObject.SetActive(false);
        gameOverWindow.alpha = 0f;
        gameOverWindow.gameObject.SetActive(false);
    }
}
