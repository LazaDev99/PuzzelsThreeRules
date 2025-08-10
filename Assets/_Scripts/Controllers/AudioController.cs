using System.Collections.Generic;
using Game.Scriptables;
using Game.Utilities;
using UnityEngine;

namespace Game.Controllers
{
    /// <summary>
    /// Manages audio playback for sound effects and music in the game.
    /// </summary>
    public class AudioController : Singleton<AudioController>
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private int sfxSourceSize = 5;
        private List<AudioSource> sfxSources;

        // Dictionary to keep track of active SFX sources, mapping AudioClip to AudioSource just to avoid brute force searching.
        private Dictionary<AudioClip, AudioSource> activeSFXSources;

        protected override void Awake()
        {
            base.Awake();
            InitSFXPool();
            InitSoundAndVolume();
        }



        /// <summary>
        /// Plays the sound effect or music based on the provided AudioData.
        /// AudioData should contain the AudioClip, volume, pitch, loop settings, and category (SFX or Music for this game).
        /// </summary>
        /// <param name="data"></param>
        public void PlaySound(AudioData data)
        {
            if (data == null || data.audioClip == null)
            {
                Debug.Log("Data or AudioClip is null, cannot play sound effect.");
                return;
            }

            switch (data.audioCategory)
            {
                case AudioCategory.SFX:
                    PlaySFX(data);
                    break;

                case AudioCategory.Music:
                    PlayMusic(data);
                    break;

                default:
                    PlaySFX(data);
                    break;
            }
        }


        /// <summary>
        /// Stops the sound effect or music based on the provided AudioData.
        /// </summary>
        /// <param name="data"></param>
        public void StopSound(AudioData data)
        {
            if (data == null || data.audioClip == null)
            {
                Debug.Log("Data or AudioClip is null, cannot stop sound effect.");
                return;
            }

            switch (data.audioCategory)
            {
                case AudioCategory.SFX:
                    StopSFX(data);
                    break;
                case AudioCategory.Music:
                    StopMusic(data);
                    break;
                default:
                    StopSFX(data);
                    break;
            }
        }



        /// <summary>
        /// Based on the settings, enables or disables sound effects.
        /// Toogle is responsible for setting the volume of all SFX sources.
        /// </summary>
        /// <param name="isEnabled"></param>
        public void SFXOnOff(bool isEnabled)
        {
            foreach (var source in sfxSources)
            {
                if (source != null)
                    source.volume = isEnabled ? 1 : 0;
            }
        }



        /// <summary>
        /// Adjusts the music volume based on the provided value.
        /// Slider is responsible for setting the volume of the music source.
        /// </summary>
        /// <param name="volume"></param>
        public void MusicVolume(float volume)
        {
            if (musicSource != null)
                musicSource.volume = volume;
        }



        /// <summary>
        /// Returns whether the music is currently playing.
        /// </summary>
        /// <returns></returns>
        public bool IsMusicPlaying()
        {
            if (musicSource == null)
                return false;

            return musicSource.isPlaying;
        }



        #region AUDIO_SYSTEM_PRIVATE_METHODS

        /// <summary>
        /// Sets the music source properties and plays the music.
        /// </summary>
        /// <param name="data"></param>
        private void PlayMusic(AudioData data)
        {
            if (musicSource == null || musicSource.isPlaying) return;

            musicSource.clip = data.audioClip;
            musicSource.pitch = data.pitch;
            musicSource.loop = data.loop;
            musicSource.Play();
        }



        /// <summary>
        /// Sets the sound effect source properties and plays the sound effect.
        /// Tcks the active sound effect sources in a dictionary to avoid brute force searching.
        /// </summary>
        /// <param name="data"></param>
        private void PlaySFX(AudioData data)
        {
            AudioSource source = GetAvailableSFXSource();
            if (source == null || source.volume == 0) return;

            source.clip = data.audioClip;
            source.volume = data.volume;
            source.pitch = data.pitch;
            source.loop = data.loop;

            source.PlayOneShot(data.audioClip);

            if (!activeSFXSources.ContainsKey(data.audioClip))
                activeSFXSources.Add(data.audioClip, source);
            else
                activeSFXSources[data.audioClip] = source;
        }



        /// <summary>
        /// Stops the music if it is currently playing and matches the provided AudioData.
        /// </summary>
        /// <param name="data"></param>
        private void StopMusic(AudioData data)
        {
            if (musicSource != null && musicSource.isPlaying && musicSource.clip == data.audioClip)
            {
                musicSource.Stop();
            }
        }

        // Stops the sound effect if it is currently playing and matches the provided AudioData.
        // Use dictionary to track active SFX sources and avoid brute force searching.
        private void StopSFX(AudioData data)
        {
            if (activeSFXSources.TryGetValue(data.audioClip, out AudioSource source))
            {
                if (source != null && source.isPlaying)
                {
                    source.Stop();
                    activeSFXSources.Remove(data.audioClip);
                    return;
                }
            }

            Debug.Log($"Clip {data.audioClip.name} not currently tracked as playing.");
        }



        /// <summary>
        /// Initializes the SFX pool with a specified number of AudioSources.
        /// This pool is used to manage sound effects efficiently, allowing for reuse of AudioSources and overlapping sounds.
        /// </summary>
        private void InitSFXPool()
        {
            Transform sfx = transform.Find("SFX");
            if (sfx == null)
            {
                sfx = new GameObject("SFX").transform;
                sfx.SetParent(transform, false);
            }

            sfxSources = new List<AudioSource>(sfxSourceSize);
            activeSFXSources = new Dictionary<AudioClip, AudioSource>(sfxSourceSize);

            for (int i = 0; i < sfxSourceSize; i++)
            {
                GameObject sfxObject = new GameObject($"SFXAudioSource_{i + 1}");
                sfxObject.transform.SetParent(sfx, false);

                AudioSource newSource = sfxObject.AddComponent<AudioSource>();
                newSource.playOnAwake = false;
                sfxSources.Add(newSource);
            }
        }



        /// <summary>
        /// Retrieves an available AudioSource from the SFX pool that is not currently playing.
        /// </summary>
        /// <returns></returns>
        private AudioSource GetAvailableSFXSource()
        {
            foreach (var source in sfxSources)
            {
                if (!source.isPlaying)
                    return source;
            }

            Debug.Log("No available SFX source found in the pool.");
            return null;
        }



        /// <summary>
        /// Initializes sound effects and music volume based on saved preferences.
        /// </summary>
        private void InitSoundAndVolume()
        {
            SFXOnOff(PlayerPrefs.GetInt("sound_enabled", 1) == 1);
            MusicVolume(PlayerPrefs.GetFloat("music_volume", 1.0f));
        }

        #endregion

    }
}
