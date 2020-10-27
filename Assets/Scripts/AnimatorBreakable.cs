using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RasPacJam.Audio;

public class AnimatorBreakable : MonoBehaviour
{
    [SerializeField] private bool isStartAnimation = false;
    [SerializeField] private Vector3 addedHeight = new Vector3(0f, 100f, 0f);
    [SerializeField] private float fallTime = 1f;
    [SerializeField] private float rotatingSpeed = 0.05f;
    [SerializeField] private Vector3 rotation = new Vector3(0f, 0f, 2f);
    [SerializeField] private float explosionDelay = 0.3f;
    [SerializeField] private ParticleSystem explosionParticlesPrefab = null;
    private Tween rotationTween;
    private Coroutine falling;



    public void StartBreaking()
    {
        GetComponent<Collider>().enabled = false;
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
                            GetComponent<Collider>().enabled = true;
                            Explode();
                        }
                );
    }

    public void Explode()
    {
        Instantiate(explosionParticlesPrefab,
                GetComponent<Renderer>().bounds.center, Quaternion.identity);
        if(isStartAnimation)
        {
            Despawn();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Despawn()
    {
        if (falling != null)
        {
            StopCoroutine(falling);
        }

        transform.position = transform.position + addedHeight;
        falling = StartCoroutine(Fall());
    }



    private IEnumerator Fall()
    {
        float time = 0;
        Vector3 startingPos = transform.position;
        Vector3 endingPos = transform.position - addedHeight;
        float tRatio;
        while(time < fallTime)
        {
            tRatio = time / fallTime;
            transform.position = Vector3.Lerp(startingPos, endingPos, tRatio);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = endingPos;
    }

    private void Start()
    {
        rotationTween = transform.DOLocalRotate(rotation, rotatingSpeed).SetLoops(-1, LoopType.Yoyo);
    }
}
