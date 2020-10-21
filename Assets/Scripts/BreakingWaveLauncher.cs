using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using RasPacJam.Audio;

public class BreakingWaveLauncher : MonoBehaviour
{
    public OnObstacleBrokenEvent OnObstacleBroken => onObstacleBroken;

    [SerializeField] private float breakingWaveDuration = 1f;
    [SerializeField] private float breakingWaveRadius = 10f;
    [SerializeField] private ParticleSystem wavePrefab = null;
    [SerializeField] private SphereCollider playerCollider = null;
    private OnObstacleBrokenEvent onObstacleBroken;
    private SphereCollider waveField;
    private ParticleSystem wave;
    private float initialRadius;



    public void Launch()
    {
        AudioManager.Instance.Play("breakingWaveLaunch");
        wave = Instantiate(wavePrefab, transform);
        var shape = wave.shape;
        shape.radius = transform.parent.localScale.z * playerCollider.transform.localScale.z * playerCollider.radius;
        var main = wave.main;
        main.startLifetime = breakingWaveDuration;
        main.startSpeed = (breakingWaveRadius - initialRadius) / breakingWaveDuration;
        DOTween.To(() => waveField.radius, value => waveField.radius = value,
                breakingWaveRadius, breakingWaveDuration).SetEase(Ease.Linear).OnComplete(ResetWave);
    }



    private void Awake()
    {
        waveField = GetComponent<SphereCollider>();
        onObstacleBroken = new OnObstacleBrokenEvent();
    }

    private void Start()
    {
        initialRadius = waveField.radius;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            onObstacleBroken.Invoke(other);
            if(other.TryGetComponent<AnimatorBreakable>(out AnimatorBreakable obstacle))
            {
                obstacle.StartBreaking();
            }
        }
    }

    private void ResetWave()
    {
        waveField.radius = initialRadius;
        gameObject.SetActive(false);
        Destroy(wave.gameObject);

    }

    public class OnObstacleBrokenEvent : UnityEvent<Collider> { }
}
