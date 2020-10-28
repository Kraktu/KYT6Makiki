using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using RasPacJam.Audio;

public class LightsManager : MonoBehaviour
{
    public List<LightsGroup> Lights => lights;

    [SerializeField] private List<LightsGroup> lights = null;
    [SerializeField] private RectTransform uiStarsPanel = null;
    [SerializeField] private TextMeshProUGUI uiStarsCount = null;
    [SerializeField] private TextMeshProUGUI uistarsCountForNextLevel = null;
    [SerializeField] private int starsCountForNextLevel = 3;
    [SerializeField] private float uiScaleIntensity = 2f;
    [SerializeField] private float uiScaleDuration = 0.5f;
    private int starsCount;



    public void PickUpStar()
    {
        starsCount++;
        int relativeStarsCount = starsCount % starsCountForNextLevel;
        if(relativeStarsCount == 0)
        {
            int lightsGroupIndex = (starsCount / starsCountForNextLevel);
            if (lightsGroupIndex >= lights.Count)
            {
                return;
            }
            foreach(Light light in lights[lightsGroupIndex].List)
            {
                light.gameObject.SetActive(true);
            }
            AudioManager.Instance.Play("nextLightingLevel");
            uiStarsCount.text = starsCountForNextLevel.ToString();
            Vector3 initialScale = uiStarsPanel.localScale;
            Vector3 finalScale =  initialScale * uiScaleIntensity;
            uiStarsPanel
                    .DOScale(finalScale, uiScaleDuration)
                    .OnComplete(() =>
                    {
                        uiStarsPanel.DOScale(initialScale, uiScaleDuration);
                        uiStarsCount.text = "0";
                    });
        }
        else
        {
            uiStarsCount.text = relativeStarsCount.ToString();
        }
        AudioManager.Instance.Play("starPickedUp");
    }

    public void SwitchLights(Sequence sequence, bool isSwitchingOn, float delay)
    {
        foreach(LightsGroup lightsGroup in lights)
        {
            for(int i = 0 ; i < lightsGroup.List.Count ; i++)
            {
                float intensity = isSwitchingOn ? lightsGroup.Intensities[i] : 0f;
                sequence.Insert(0f, lightsGroup.List[i].DOIntensity(intensity, delay));
            }
        }
    }



    private void Awake()
    {
        starsCount = 0;
    }

    private void Start()
    {
        for(int i = 0 ; i < lights[0].List.Count ; i++)
        {
            Light light = lights[0].List[i];
            light.gameObject.SetActive(true);
            lights[0].Intensities.Add(light.intensity);
        }

        for(int i = 1 ; i < lights.Count ; i++)
        {
            List<Light> lightsList = lights[i].List;
            for(int j = 0 ; j < lightsList.Count ; j++)
            {
                Light light = lightsList[j];
                light.gameObject.SetActive(false);
                lights[i].Intensities.Add(light.intensity);
            }
        }

        uiStarsCount.text = "0";
        uistarsCountForNextLevel.text = starsCountForNextLevel.ToString();
    }

    [System.Serializable]
    public class LightsGroup
    {
        public List<Light> List => list;
        public List<float> Intensities => intensities;

        [SerializeField] private List<Light> list = null;
        private List<float> intensities = new List<float>();
    }
}
