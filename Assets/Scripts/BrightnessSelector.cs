using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class BrightnessSelector : MonoBehaviour
{
    [SerializeField] private PostProcessVolume postProcessVolume = null;
    [SerializeField] private Slider brightnessSlider = null;
    private ColorGrading colorGrading;



    private void Awake()
    {
        DontDestroyOnLoad(postProcessVolume);
    }

    private void Start()
    {
        postProcessVolume.profile.TryGetSettings<ColorGrading>(out colorGrading);
    }

    private void Update()
    {
        if(colorGrading)
        {
            colorGrading.brightness.value = brightnessSlider.value * 200 - 100;
        }
    }
}
