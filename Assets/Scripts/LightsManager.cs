using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RasPacJam.Audio;

public class LightsManager : MonoBehaviour
{
    public List<LightsGroup> Lights => lights;

    [SerializeField] private List<LightsGroup> lights = null;
    [SerializeField] private int starsCountForNextLightingLevel = 3;
    private int starsCount;



    public void PickUpStar()
    {
        starsCount++;
        if(starsCount % starsCountForNextLightingLevel == 0)
        {
            int lightsGroupIndex = (starsCount / starsCountForNextLightingLevel);
			if (lightsGroupIndex>=lights.Count)
			{
				return;
			}
            foreach(Light light in lights[lightsGroupIndex].List)
            {
                light.gameObject.SetActive(true);
            }
            AudioManager.Instance.Play("nextLightingLevel");
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
