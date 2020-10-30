using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform target = null;
    [SerializeField] private Vector3 nearOffset = new Vector3(0f, 5f, -15f);
    [SerializeField] private Vector3 farOffset = new Vector3(0f, 5f, -40f);
    [SerializeField] private Vector3 initialOffset = new Vector3(0f, 0f, 0f);
    [SerializeField] private float zoomDuration = 1f;
    [SerializeField] private bool isLookingTowardsPlayer = false;
    [SerializeField] private bool isSmooth = false;
    [SerializeField] private float smoothSpeed = 0.1f;
    private Vector3 offset;
    private Tween cameraTween;



    public void Zoom(bool isZoomingIn)
    {
        if(cameraTween != null)
        {
            cameraTween.Kill();
        }

        offset = isZoomingIn ? nearOffset : farOffset;
    }


    private void Awake()
    {
        offset = nearOffset;
        transform.position = target.position + initialOffset;
    }

    private void LateUpdate()
    {
        Vector3 finalPosition = target.position + offset;
        if(isSmooth)
        {
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, finalPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
        else
        {
            transform.position = finalPosition;
        }

        if(isLookingTowardsPlayer)
        {
            transform.LookAt(target);
        }
    }
}
