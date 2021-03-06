﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using RasPacJam.Audio;

public class AutoRun : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float deathDelay = 1f;
    [SerializeField] private float revivingDelay = 0.3f;
    [SerializeField] private LightsManager lightsManager = null;
    [SerializeField] private StuckChecker stuckChecker = null;
    [SerializeField] private RoofChecker roofChecker = null;
    [SerializeField] private DeathZoneDetector deathZoneDetector = null;
    [SerializeField] private Rotator model = null;
    [SerializeField] private ParticleSystem cloudParticlesPrefab = null;
    [SerializeField] private ParticleSystem burstParticlesPrefab = null;
    [SerializeField] private Transform particlesLocation = null;
    [SerializeField] private UnityEvent onStopped = null;
    [SerializeField] private UnityEvent onUnstopped = null;
    [SerializeField] private UnityEvent onDead = null;
    [SerializeField] private UnityEvent onReset = null;
    private Vector3 startPosition;
    private Rigidbody rb;
    private ParticleSystem cloudParticles;
    private Sequence dyingSequence;
    private bool isDying;
    private bool isStopped;



    public void SetStopped(bool isStopped)
    {
        this.isStopped = isStopped;
        if(isStopped)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);
            model.CanRotate = false;
            onStopped.Invoke();
        }
        else
        {
            model.CanRotate = true;
            onUnstopped.Invoke();
        }
    }

    public void StartDying()
    {
        if(isDying)
        {
            return;
        }

        isDying = true;
        if(dyingSequence != null && dyingSequence.IsActive())
        {
            dyingSequence.Kill();
        }
        dyingSequence = DOTween.Sequence();

        AudioManager.Instance.SwitchReverb(true, false, dyingSequence, deathDelay);
        lightsManager.SwitchLights(dyingSequence, false, deathDelay);
        dyingSequence.OnComplete(Die);
    }

    public void StopDying()
    {
        if(!isDying)
        {
            return;
        }

        isDying = false;
        if(dyingSequence != null && dyingSequence.IsActive())
        {
            dyingSequence.Kill();
        }

        dyingSequence = DOTween.Sequence();

        AudioManager.Instance.SwitchReverb(false, false, dyingSequence, revivingDelay);
        lightsManager.SwitchLights(dyingSequence, true, revivingDelay);
    }

    public void Die()
    {
        Sequence deathSequence = DOTween.Sequence();
        lightsManager.SwitchLights(deathSequence, true, revivingDelay);
        AudioManager.Instance.SwitchReverb(true, true, deathSequence, revivingDelay);
        AudioManager.Instance.Play("death");
        isDying = false;
        PlayDeathAnimation();
        onDead.Invoke();
    }

    public void PickUpStar()
    {
        lightsManager.PickUpStar();
    }



    private void Awake()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        dyingSequence = null;
        isStopped = false;
        isDying = false;
    }

    private void Update()
    {
        if(isStopped)
        {
            return;
        }

        rb.velocity = new Vector3(speed, rb.velocity.y, rb.velocity.z);
    }

    private void PlayDeathAnimation()
    {
        if(dyingSequence != null && dyingSequence.IsActive())
        {
            dyingSequence.Kill();
        }
        SetStopped(true);
        foreach(Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
        rb.isKinematic = true;
        stuckChecker.gameObject.SetActive(false);
        deathZoneDetector.gameObject.SetActive(false);
        cloudParticles = Instantiate(cloudParticlesPrefab, particlesLocation);
        Sequence deathAnimSequence = DOTween.Sequence();
        deathAnimSequence
                .Append(transform.DOLocalMoveZ(-10f, 2f)
                        .SetEase(Ease.InOutBack))
                .Append(transform.DOMove(startPosition + new Vector3(0f, 0f, -10f), 2f)
                        .SetEase(Ease.InOutBack))
                .Append(transform.DOMove(startPosition, 2f)
                        .SetEase(Ease.InOutBack))
                .OnComplete(FinalizeAnimation);
    }

    private void FinalizeAnimation()
    {
        Sequence resetSequence = DOTween.Sequence();
        AudioManager.Instance.ResetMusic();
        Destroy(cloudParticles.gameObject);
        Instantiate(burstParticlesPrefab, particlesLocation);
        deathZoneDetector.gameObject.SetActive(true);
        stuckChecker.gameObject.SetActive(true);
        stuckChecker.Reset();
        roofChecker.gameObject.SetActive(true);
        roofChecker.Reset();
        rb.isKinematic = false;
        foreach(Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = true;
        }
        roofChecker.gameObject.SetActive(false);
        onReset.Invoke();
        SetStopped(false);
    }
}
