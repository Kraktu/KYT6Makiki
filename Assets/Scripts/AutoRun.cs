using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using RasPacJam.Audio;

public class AutoRun : MonoBehaviour
{
    public bool IsEnabled
    {
        get => isEnabled;
        set
        {
            isEnabled = value;
            model.CanRotate = value;
        }
    }

    [SerializeField] private float speed = 10f;
    [SerializeField] private float deathDelay = 1f;
    [SerializeField] private float revivingDelay = 0.3f;
    [SerializeField] private LightsManager lightsManager = null;
    [SerializeField] private StuckChecker stuckChecker = null;
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
    private Sequence deathSequence;
    private bool isStopped;
    private bool isEnabled;

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
        if(deathSequence != null && deathSequence.IsActive())
        {
            deathSequence.Kill();
        }
        else
        {
            deathSequence = DOTween.Sequence();
        }

        deathSequence
                .Insert(0f, AudioManager.Instance.GetMusic(MusicName.Main).DOFade(0f, deathDelay))
                .Insert(0f, AudioManager.Instance.GetMusic(MusicName.Reverb).DOFade(0.5f, deathDelay));
        lightsManager.SwitchLights(deathSequence, false, deathDelay);
        deathSequence.OnComplete(Die);
    }

    public void StopDying()
    {
        if(deathSequence != null && deathSequence.IsActive())
        {
            deathSequence.Kill();
        }
        else
        {
            deathSequence = DOTween.Sequence();
        }

        deathSequence
                .Insert(0f, AudioManager.Instance.GetMusic(MusicName.Reverb).DOFade(0f, revivingDelay))
                .Insert(0f, AudioManager.Instance.GetMusic(MusicName.Main).DOFade(0.5f, revivingDelay));
        lightsManager.SwitchLights(deathSequence, true, revivingDelay);
    }

    public void Die()
    {
        PlayDeathAnimation();
        lightsManager.SwitchLights(DOTween.Sequence(), true, revivingDelay);
        AudioManager.Instance.Play("death");
        cloudParticles = Instantiate(cloudParticlesPrefab, particlesLocation);
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
        deathSequence = null;
        isStopped = false;
        isEnabled = true;
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
        foreach(Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
        SetStopped(true);
        rb.isKinematic = true;
        stuckChecker.gameObject.SetActive(false);

        Sequence deathAnimSequence = DOTween.Sequence();
        deathAnimSequence
                .Append(transform.DOLocalMoveZ(-8f, 2f)
                        .SetEase(Ease.InOutBack))
                .Append(transform.DOMove(startPosition + new Vector3(0f,0f,-8f), 2f)
                        .SetEase(Ease.InOutBack))
                .Append(transform.DOMove(startPosition, 2f)
                        .SetEase(Ease.InOutBack))
                .OnComplete(ResetAutoRun);
    }

    private void ResetAutoRun()
    {
        stuckChecker.Reset();
        foreach(Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = true;
        }
        SetStopped(false);
        rb.isKinematic = false;
        Destroy(cloudParticles.gameObject);
        Instantiate(burstParticlesPrefab, particlesLocation);
        onReset.Invoke();

        Sequence resetSequence = DOTween.Sequence();
        resetSequence
                .Insert(0f, AudioManager.Instance.GetMusic(MusicName.Main).DOFade(0.5f, 0.3f))
                .Insert(0f, AudioManager.Instance.GetMusic(MusicName.Reverb).DOFade(0f, 0.3f));
    }
}
