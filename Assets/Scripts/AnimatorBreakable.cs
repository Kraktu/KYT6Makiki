using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RasPacJam.Audio;

public class AnimatorBreakable : MonoBehaviour
{
    [SerializeField] private float rotatingSpeed = 0.05f;
    [SerializeField] private Vector3 rotation = new Vector3(0f, 0f, 2f);
    [SerializeField] private float explosionDelay = 0.3f;
    [SerializeField] private ParticleSystem explosionParticlesPrefab = null;
    private Tween rotationTween;



    public void StartBreaking()
    {
        AudioManager.Instance.Play("obstacleDestruction");
        rotationTween.Kill();
        Vector3 extents = GetComponent<Renderer>().bounds.extents;
        DOTween.Sequence()
                .Insert(0f, transform.DOScale(new Vector3(2f, 2f, 1f), explosionDelay))
                .Insert(0f, transform.DOLocalMove(new Vector3(extents.x / 2, - extents.y, 0f), explosionDelay))
                .OnComplete(() =>
                        {
                            transform.localScale = Vector3.one;
                            transform.localPosition = Vector3.zero;
                            Explode();
                        }
                );
    }

    public void Explode()
    {
        Instantiate(explosionParticlesPrefab,
                GetComponent<Renderer>().bounds.center, Quaternion.identity);
        gameObject.SetActive(false);
    }



    private void Start()
    {
        rotationTween = transform.DOLocalRotate(rotation, rotatingSpeed).SetLoops(-1, LoopType.Yoyo);
    }
}
