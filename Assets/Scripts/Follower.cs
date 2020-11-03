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
    [SerializeField] private bool isLookingTowardsPlayer = false;
    [SerializeField] private bool isSmooth = false;
    [SerializeField] private float smoothDuration = 5f;
    private Tweener cameraZoomTween;
    private Vector3 offset;
    private Vector3 velocity;



    public void Zoom(bool isZoomingIn)
    {
        if(cameraZoomTween != null)
        {
            cameraZoomTween.Kill();
        }

        offset = isZoomingIn ? nearOffset : farOffset;
    }


    private void Awake()
    {
        cameraZoomTween = null;
        transform.position = target.position + initialOffset;
        offset = nearOffset;
        velocity = Vector3.zero;
    }

    private void LateUpdate()
    {
        Vector3 finalPosition = target.position + offset;
        if(isSmooth)
        {
            transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref velocity, smoothDuration);
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
