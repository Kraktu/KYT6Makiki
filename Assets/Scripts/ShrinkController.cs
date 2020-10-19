using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RasPacJam.Audio;

public class ShrinkController : MonoBehaviour
{
    public bool CanShrink { get => canShrink; set => canShrink = value; }

    [SerializeField] private float shrinkedTime = 2f;
    [SerializeField] private float shrinkedSizeFactor = 0.5f;
    [SerializeField] private RoofChecker roofChecker = null;
    private bool isShrinked;
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

        transform.localScale /= shrinkedSizeFactor;
        AudioManager.Instance.Play("growing");
        isShrinked = false;
        roofChecker.gameObject.SetActive(false);
    }



    private void Awake()
    {
        isShrinked = false;
        canShrink = true;
    }

    private void Start()
    {
        roofChecker.gameObject.SetActive(false);
    }

    private IEnumerator StartShrinking()
    {
        AudioManager.Instance.Play("shrinking");
        roofChecker.gameObject.SetActive(true);
        isShrinked = true;
        transform.localScale *= shrinkedSizeFactor;
        float time = 0;

        while(time < shrinkedTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        if(!roofChecker.IsRoofed)
        {
            Grow();
        }
    }
}
