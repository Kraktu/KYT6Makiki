using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using RasPacJam.Audio;

public class BrightnessSelector : MonoBehaviour
{
    [SerializeField] private PostProcessVolume postProcessVolumePrefab = null;
    [SerializeField] private Slider brightnessSlider = null;
    private ColorGrading colorGrading;
    private PostProcessVolume postProcessVolume;


    private void Awake()
    {
        postProcessVolume = FindObjectOfType<PostProcessVolume>();
        if(!postProcessVolume)
        {
            postProcessVolume = Instantiate(postProcessVolumePrefab);
            DontDestroyOnLoad(postProcessVolume);
        }
    }

    private void Start()
    {
        postProcessVolume.profile.TryGetSettings<ColorGrading>(out colorGrading);
        AudioManager.Instance.PlayOnlyDrums();
    }

    private void Update()
    {
        if(colorGrading)
        {
            colorGrading.brightness.value = brightnessSlider.value * 200 - 100;
        }
    }
}
