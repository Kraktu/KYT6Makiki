using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RasPacJam.Audio;

public class ShrinkController : MonoBehaviour
{
    public bool IsShrinked => isShrinked;
    public bool CanShrink { get => canShrink; set => canShrink = value; }

    [SerializeField] private float shrinkedTime = 2f;
    [SerializeField] private float shrinkedSizeFactor = 0.5f;
    [SerializeField] private float shrinkDuration= 0.5f;
    [SerializeField] private RoofChecker roofChecker = null;
    private Vector3 initialSize;
    private bool isShrinked;
    private bool isShrinkTimerFinished;
    private bool canShrink;



    public void Shrink()
    {
        if(isShrinked)
        {
            return;
        }
        if(!canShrink)
        {
            return;
        }

        StartCoroutine(StartShrinking());
    }

    public void Grow()
    {
        if(!isShrinked)
        {
            return;
        }
        if(roofChecker.IsInShrinkCollider)
        {
            return;
        }
        if(canShrink && !isShrinkTimerFinished)
        {
            return;
        }

        transform
                .DOScale(initialSize, shrinkDuration)
                .SetEase(Ease.OutBounce)
                .OnComplete(() => isShrinked = false);
        AudioManager.Instance.Play("growing");
        roofChecker.gameObject.SetActive(false);
    }



    private void Awake()
    {
        initialSize = transform.localScale;
        isShrinked = false;
        isShrinkTimerFinished = false;
        canShrink = true;
    }

    private void Start()
    {
        roofChecker.gameObject.SetActive(false);
    }

    private IEnumerator StartShrinking()
    {
        isShrinkTimerFinished = false;
        AudioManager.Instance.Play("shrinking");
        roofChecker.gameObject.SetActive(true);
        isShrinked = true;
        transform.DOScale(initialSize * shrinkedSizeFactor, shrinkDuration).SetEase(Ease.OutBounce);
        float time = 0;

        while(time < shrinkedTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        isShrinkTimerFinished = true;

        if(!roofChecker.IsRoofed && !roofChecker.IsInShrinkCollider)
        {
            Grow();
        }
    }
}
