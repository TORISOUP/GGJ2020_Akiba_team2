using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2020.Managers
{
    public class BGMFadeOut : MonoBehaviour
    {
        private AudioSource audioSource;
        private float fadeTime;
        private float startVolume;
        private float volumeDelta;
        private float setVolume;

        public void Init(AudioSource _audioSource, float _fadeTime)
        {
            audioSource = _audioSource;
            fadeTime = _fadeTime;
            startVolume = audioSource.volume;
            volumeDelta = startVolume / fadeTime;
            setVolume = startVolume;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (audioSource.volume > 0)
            {
                setVolume -= volumeDelta * Time.deltaTime;
                audioSource.volume = setVolume;
            }
            else
            {
                audioSource.volume = 0;
            }
        }
    }
}


