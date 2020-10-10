using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RasPacJam.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance => instance;

        [SerializeField] private List<Sound> sounds = null;
        public AudioSource music = null;
        public AudioSource musicReverb = null;
        private static AudioManager instance;
        private Dictionary<string, float> lastPlayedTimes;

        public void Play(string soundName, float delay = 0f)
        {
            if(CanPlay(soundName, delay))
            {
                AudioSource source = new GameObject("SoundPlayer").AddComponent<AudioSource>();
                Sound sound = GetSoundByName(soundName);
                source.volume = sound.Volume;
                source.pitch = sound.Pitch;
                source.PlayOneShot(sound.Clip);

                Destroy(source.gameObject, sound.Clip.length);
            }
        }



        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            lastPlayedTimes = new Dictionary<string, float>();
            foreach(Sound sound in sounds)
            {
                lastPlayedTimes[sound.Name] = 0f;
            }
        }

        private Sound GetSoundByName(string soundName)
        {
            List<Sound> similarSounds = sounds.FindAll(sound => sound.Name == soundName);
            return similarSounds[Random.Range(0, similarSounds.Count - 1)];
        }

        private bool CanPlay(string soundName, float delay)
        {
            if(lastPlayedTimes[soundName] == 0f || Time.time > lastPlayedTimes[soundName] + delay)
            {
                lastPlayedTimes[soundName] = Time.time;
                return true;
            }
            return false;
        }
    }



    [System.Serializable]
    public class Sound
    {
        public string Name => name;
        public AudioClip Clip => clip;
        public float Volume => volume;
        public float Pitch => pitch;

        [SerializeField] private string name = null;
        [SerializeField] private AudioClip clip = null;
        [SerializeField] [Range(0f, 1f)] private float volume = 1f;
        [SerializeField] [Range(0.1f, 3f)] private float pitch = 1f;
    }
}