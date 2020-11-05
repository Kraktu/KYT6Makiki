using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace RasPacJam.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance => instance;
        private static AudioManager instance;

        [SerializeField] private List<Sound> sounds = null;
        [SerializeField] private AudioSource music = null;
        [SerializeField] private AudioSource reverb = null;
        private Dictionary<string, AudioSource> sources;
        private Dictionary<string, float> lastPlayedTimes;
        private float musicActiveVolume;
        private float reverbActiveVolume;

        public void Play(string soundName, float delay = 0f)
        {
            if(CanPlay(soundName, delay))
            {
                if(!sources.TryGetValue(soundName, out AudioSource source))
                {
                    source = new GameObject("SoundPlayer").AddComponent<AudioSource>();
                    sources.Add(soundName, source);
                    DontDestroyOnLoad(source);
                }
                if(source.isPlaying)
                {
                    source.Stop();
                }
                Sound sound = GetSoundByName(soundName);
                source.volume = sound.Volume;
                source.pitch = sound.Pitch;
                source.PlayOneShot(sound.Clip);
            }
        }

        public void Stop(string soundName)
        {
            if(sources.TryGetValue(soundName, out AudioSource source))
            {
                source.Stop();
            }
        }

        public void SwitchReverb(bool isReverb, Sequence sequence = null, float duration = 0f)
        {
            float finalMusicVolume = 0f;
            float finalReverbVolume = 0f;
            if(isReverb)
            {
                finalMusicVolume = 0f;
                finalReverbVolume = reverbActiveVolume;
            }
            else
            {
                finalMusicVolume = musicActiveVolume;
                finalReverbVolume = 0f;
            }

            if(sequence != null)
            {
                sequence
                        .Insert(0f, music.DOFade(finalMusicVolume, duration))
                        .Insert(0f, reverb.DOFade(finalReverbVolume, duration));
            }
            else
            {
                music.volume = finalMusicVolume;
                reverb.volume = finalReverbVolume;
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

            sources = new Dictionary<string, AudioSource>();

            lastPlayedTimes = new Dictionary<string, float>();
            foreach(Sound sound in sounds)
            {
                lastPlayedTimes[sound.Name] = 0f;
            }

            musicActiveVolume = music.volume;
            music.volume = 0f;

            reverbActiveVolume = reverb.volume;
            reverb.volume = 0f;

            DontDestroyOnLoad(this);
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