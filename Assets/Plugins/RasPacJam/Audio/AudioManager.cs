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
        [SerializeField] private List<AudioSource> drums = null;
        [SerializeField] private List<AudioSource> drumsReverbs = null;
        [SerializeField] private List<AudioSource> musics = null;
        [SerializeField] private List<AudioSource> reverbs = null;
        private Dictionary<string, AudioSource> sources;
        private Dictionary<string, float> lastPlayedTimes;
        private AudioSource currentMusic;
        private AudioSource currentReverb;
        private float drumActiveVolume;
        private float drumReverbActiveVolume;
        private float musicActiveVolume;
        private float activeReverbVolume;
        private float loopStartTime;
        private Coroutine waitForMusic;

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

        public void SwitchReverb(bool isReverb, bool isOnlyDrums, Sequence sequence = null, float duration = 0f)
        {
            float finalDrumVolume = 0f;
            float finalDrumReverbVolume = 0f;
            float finalMusicVolume = 0f;
            float finalReverbVolume = 0f;
            if(isReverb)
            {
                finalDrumVolume = 0f;
                finalDrumReverbVolume = drumReverbActiveVolume;
                if(!isOnlyDrums && currentMusic.volume > 0f)
                {
                    finalMusicVolume = 0f;
                    finalReverbVolume = activeReverbVolume;
                }
                else
                {
                    StopCoroutine(waitForMusic);
                }
            }
            else
            {
                finalDrumVolume = drumActiveVolume;
                finalDrumReverbVolume = 0f;
                if(currentReverb.volume > 0f)
                {
                    finalMusicVolume = musicActiveVolume;
                    finalReverbVolume = 0f;
                }
                else
                {
                    ResetMusic();
                }
            }

            if(sequence != null)
            {
                foreach(AudioSource source in drums)
                {
                    sequence.Insert(0f, source.DOFade(finalDrumVolume, duration));
                }
                foreach(AudioSource source in drumsReverbs)
                {
                    sequence.Insert(0f, source.DOFade(finalDrumReverbVolume, duration));
                }

                sequence
                        .Insert(0f, currentMusic.DOFade(finalMusicVolume, duration))
                        .Insert(0f, currentReverb.DOFade(finalReverbVolume, duration));
            }
            else
            {
                foreach(AudioSource source in drums)
                {
                    source.volume = finalDrumVolume;
                }
                foreach(AudioSource source in drumsReverbs)
                {
                    source.volume = finalDrumReverbVolume;
                }
                currentMusic.volume = finalMusicVolume;
                currentReverb.volume = finalReverbVolume;
            }
        }

        public void ResetMusic()
        {
            currentReverb.DOFade(0f, 3f);
            foreach(AudioSource source in drumsReverbs)
            {
                source.DOFade(0f, 3f);
            }

            foreach(AudioSource source in drums)
            {
                source.volume = drumActiveVolume;
            }
            float drumLength = drums[0].clip.length;
            float remainingLoopTime = drumLength - ((Time.time - loopStartTime) % drumLength);
            waitForMusic = StartCoroutine(PickNewMusic(remainingLoopTime + drumLength));
        }

        public void PlayOnlyDrums()
        {
            foreach(AudioSource source in drums)
            {
                source.volume = drumActiveVolume;
            }
            currentMusic.volume = 0f;
            currentReverb.volume = 0f;
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

            if(drums.Count > 0)
            {
                drumActiveVolume = drums[0].volume;
                foreach(AudioSource source in drums)
                {
                    source.volume = 0f;
                }
            }

            if(drumsReverbs.Count > 0)
            {
                drumReverbActiveVolume = drumsReverbs[0].volume;
                foreach(AudioSource source in drumsReverbs)
                {
                    source.volume = 0f;
                }
            }

            if(musics.Count > 0)
            {
                musicActiveVolume = musics[0].volume;
                foreach(AudioSource source in musics)
                {
                    source.volume = 0f;
                }
            }
            currentMusic = musics[1];

            if(reverbs.Count > 0)
            {
                activeReverbVolume = reverbs[0].volume;
                foreach(AudioSource source in reverbs)
                {
                    source.volume = 0f;
                }
            }
            currentReverb = reverbs[1];

            loopStartTime = Time.time;

            waitForMusic = null;

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

        private IEnumerator PickNewMusic(float waitingTime)
        {
            int newIndex = (int) Mathf.PingPong(musics.IndexOf(currentMusic) + 1, musics.Count - 1);
            currentMusic = musics[newIndex];
            currentReverb = reverbs[newIndex];
            loopStartTime = Time.time;
            yield return new WaitForSeconds(waitingTime);
            currentMusic.DOFade(musicActiveVolume, 5f).SetEase(Ease.Linear);
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