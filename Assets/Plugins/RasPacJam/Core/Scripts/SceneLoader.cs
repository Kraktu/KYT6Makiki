using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private UIManager uiManager = null;
    [SerializeField] private CanvasGroup loadingWindow = null;
    [SerializeField] private Slider loadingBar = null;
    [SerializeField] private bool isTrueLoading = true;
    [SerializeField] private float loadingTime = 1f;



    public void LoadLevel(int sceneToLoadIndex)
    {
        if(sceneToLoadIndex < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadSceneAsync(sceneToLoadIndex));
        }
    }



    private void Awake()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            if(loadingWindow)
            {
                loadingWindow.gameObject.SetActive(false);
                loadingBar.value = 0f;
            }
        }
        else if(uiManager)
        {
            loadingWindow.gameObject.SetActive(true);
            loadingWindow.alpha = 1f;
            loadingBar.value = 1f;
            uiManager.CloseWindow(loadingWindow);
        }
    }

    private IEnumerator LoadSceneAsync(int index)
    {
        if(loadingWindow)
        {
            loadingWindow.gameObject.SetActive(true);
            loadingWindow.alpha = 1f;
            loadingBar.value = 0f;
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        if(isTrueLoading)
        {
            while(!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                if(loadingBar)
                {
                    loadingBar.value = progress;
                }

                yield return null;
            }
        }
        else
        {
            operation.allowSceneActivation = false;
            if(loadingBar)
            {
                DOTween
                        .To(() => loadingBar.value, value => loadingBar.value = value, 1, loadingTime)
                        .SetEase(Ease.OutQuint)
                        .OnComplete(() =>
                                {
                                    operation.allowSceneActivation = true;
                                    DOTween.KillAll();
                                }
                        );
            }
        }
    }
}
