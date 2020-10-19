using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Follower : MonoBehaviour
{
    [SerializeField] private Followable target = null;
    [SerializeField] private Vector3 nearOffset = new Vector3(0f, 5f, -15f);
    [SerializeField] private Vector3 farOffset = new Vector3(0f, 5f, -40f);
    [SerializeField] private float zoomDuration = 1f;
    private Vector3 offset;
    private Tween cameraTween;



    public void Zoom(bool isZoomingOut)
    {
        if(cameraTween != null)
        {
            cameraTween.Kill();
        }
        cameraTween = DOTween.To(() => offset, value => offset = value,
                isZoomingOut ? farOffset : nearOffset, zoomDuration).SetEase(Ease.OutQuint);
    }


    private void Awake()
    {
        offset = nearOffset;
    }

    private void LateUpdate()
    {
        transform.position = target.transform.position + offset;
    }
}
