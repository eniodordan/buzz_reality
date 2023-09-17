using System;
using UnityEngine;

namespace BuzzReality
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [Space(10)]
        [SerializeField] private AudioClip levelCompleted;
        [SerializeField] private AudioClip levelFailed;

        private void Start()
        {
            PlayBackgroundTheme();
        }

        public void SetMusicVolume(float volume)
        {
            musicSource.volume = volume;
        }

        public void SetSFXVolume(float volume)
        {
            sfxSource.volume = volume;
        }
        
        private void PlayBackgroundTheme() => musicSource.Play();
        public void PlayLevelCompleted() => sfxSource.PlayOneShot(levelCompleted);
        public void PlayLevelFailed() => sfxSource.PlayOneShot(levelFailed);
    }
}
