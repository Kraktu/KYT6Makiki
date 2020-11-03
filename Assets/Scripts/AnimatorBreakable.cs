using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RasPacJam.Audio;

public class AnimatorBreakable : MonoBehaviour
{
    [SerializeField] private bool isRewinding = false;
    [SerializeField] private Vector3 addedHeight = new Vector3(0f, 100f, 0f);
    [SerializeField] private float fallTime = 1f;
    [SerializeField] private float rotatingSpeed = 0.05f;
    [SerializeField] private Vector3 rotation = new Vector3(0f, 0f, 2f);
    [SerializeField] private float scaleIntensity = 2f;
    [SerializeField] private float explosionDelay = 0.3f;
    [SerializeField] private ParticleSystem explosionParticlesPrefab = null;
    private Vector3 initialScale;
    private Quaternion initialRotation;
    private Tween rotationTween;



    public void StartBreaking()
    {
        GetComponent<Collider>().enabled = false;
        AudioManager.Instance.Play("obstacleDestruction");
        rotationTween.Kill();
        Vector3 center = GetComponent<Renderer>().bounds.center;
        Vector3 pivotMove = new Vector3(transform.parent.position.x - center.x, transform.parent.position.y - center.y, 0f) * scaleIntensity;

        DOTween.Sequence()
                .Insert(0f, transform.DOScale(new Vector3(scaleIntensity, scaleIntensity, 1f), explosionDelay))
                .Insert(0f, transform.DOLocalMove(pivotMove, explosionDelay))
                .OnComplete(Explode);
    }

    public void Explode()
    {
        Instantiate(explosionParticlesPrefab,
                GetComponent<Renderer>().bounds.center, Quaternion.identity);
        gameObject.SetActive(false);

        if(isRewinding)
        {
            Despawn();
        }
    }

    public void Despawn()
    {
        gameObject.SetActive(true);
        transform.position = transform.position + addedHeight;
        transform
                .DOMove(transform.position - addedHeight, fallTime)
                .SetEase(Ease.OutQuint);
    }



    private void Awake()
    {
        initialScale = transform.localScale;
        initialRotation = transform.localRotation;
    }

    private void OnEnable()
    {
        transform.localScale = initialScale;
        transform.localRotation = initialRotation;
        transform.localPosition = Vector3.zero;
        rotationTween = transform.DOLocalRotate(rotation, rotatingSpeed).SetLoops(-1, LoopType.Yoyo);
        GetComponent<Collider>().enabled = true;
    }
}
