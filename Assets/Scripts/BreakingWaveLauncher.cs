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
    private OnObstacleBrokenEvent onObstacleBroken;
    private SphereCollider waveField;
    private Light waveLight;



    public void Launch()
    {
        Sequence seq = DOTween.Sequence();
        seq
                .Insert(0f, DOTween.To(() => waveField.radius, value => waveField.radius = value, breakingWaveRadius, breakingWaveDuration))
                .Insert(0f, DOTween.To(() => waveLight.range, value => waveLight.range = value, breakingWaveRadius, breakingWaveDuration)
                        .OnComplete(ResetWave)
                );
    }



    private void Awake()
    {
        waveField = GetComponent<SphereCollider>();
        waveLight = GetComponent<Light>();
        onObstacleBroken = new OnObstacleBrokenEvent();
    }

    private void Start()
    {
        ResetWave();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            AudioManager.Instance.Play("obstacleDestruction");
            onObstacleBroken.Invoke(other);
            other.gameObject.SetActive(false);
        }
    }

    private void ResetWave()
    {
        waveField.radius = 0f;
        waveLight.range = 0f;
        gameObject.SetActive(false);
    }

    public class OnObstacleBrokenEvent : UnityEvent<Collider> { }
}
