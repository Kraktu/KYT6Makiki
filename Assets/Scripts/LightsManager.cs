using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using RasPacJam.Audio;

public class LightsManager : MonoBehaviour
{
    public List<LightsGroup> LightsGroups => lightsGroups;

    [SerializeField] private List<LightsGroup> lightsGroups = null;
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
            if (lightsGroupIndex >= lightsGroups.Count)
            {
                return;
            }
            foreach(Light light in lightsGroups[lightsGroupIndex].Lights)
            {
                light.gameObject.SetActive(true);
            }
			uiStarsCount.text = starsCount.ToString();
			AudioManager.Instance.Play("nextLightingLevel");
            Vector3 initialScale = uiStarsPanel.localScale;
            Vector3 finalScale =  initialScale * uiScaleIntensity;
            uiStarsPanel
                    .DOScale(finalScale, uiScaleDuration)
                    .OnComplete(() =>
                    {
                        uiStarsPanel.DOScale(initialScale, uiScaleDuration);
						uistarsCountForNextLevel.text = (starsCountForNextLevel * (lightsGroupIndex + 1)).ToString();
					});
        }
        else
        {
            uiStarsCount.text = starsCount.ToString();
        }
        AudioManager.Instance.Play("starPickedUp");
    }

    public void SwitchLights(Sequence sequence, bool isSwitchingOn, float delay)
    {
        foreach(LightsGroup lightsGroup in lightsGroups)
        {
            for(int i = 0 ; i < lightsGroup.Lights.Count ; i++)
            {
                if(lightsGroup.Lights[i])
                {
                    float intensity = isSwitchingOn ? lightsGroup.Intensities[i] : 0f;
                    sequence.Insert(0f, lightsGroup.Lights[i].DOIntensity(intensity, delay));
                }
            }
        }
    }



    private void Awake()
    {
        starsCount = 0;
    }

    private void Start()
    {
        if(lightsGroups.Count == 0)
        {
            return;
        }

        for(int i = 0 ; i < lightsGroups[0].Lights.Count ; i++)
        {
            Light light = lightsGroups[0].Lights[i];
            lightsGroups[0].Intensities.Add(0f);
            if(light)
            {
                light.gameObject.SetActive(true);
                lightsGroups[0].Intensities[i] = light.intensity;
            }
        }

        for(int i = 1 ; i < lightsGroups.Count ; i++)
        {
            List<Light> lights = lightsGroups[i].Lights;
            for(int j = 0 ; j < lights.Count ; j++)
            {
                Light light = lights[j];
                lightsGroups[i].Intensities.Add(0f);
                if(light)
                {
                    light.gameObject.SetActive(false);
                    lightsGroups[i].Intensities[j] = light.intensity;
                }
            }
        }

        uiStarsCount.text = "0";
        uistarsCountForNextLevel.text = starsCountForNextLevel.ToString();
    }

    [System.Serializable]
    public class LightsGroup
    {
        public List<Light> Lights => lights;
        public List<float> Intensities => intensities;

        [SerializeField] private List<Light> lights = null;
        private List<float> intensities = new List<float>();

    }
}
