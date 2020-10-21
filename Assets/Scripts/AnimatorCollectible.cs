using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimatorCollectible : MonoBehaviour
{
    [SerializeField] private float floatingIntensity = 0.5f;
    [SerializeField] private float floatingDuration = 0.5f;
    [SerializeField] private ParticleSystem explosionParticlesPrefab = null;
    Tween floatingTween;



    public void Explode()
    {
        floatingTween.Kill();
        Instantiate(explosionParticlesPrefab,
                GetComponent<Renderer>().bounds.center, Quaternion.identity);
        Destroy(gameObject);
    }



    private void Start()
    {
        floatingTween = transform.DOLocalMoveY(floatingIntensity, floatingDuration).SetLoops(-1, LoopType.Yoyo);
    }
}
