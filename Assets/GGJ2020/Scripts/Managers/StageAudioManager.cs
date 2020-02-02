using System;
using UnityEngine;
using Zenject;
using UniRx;

namespace GGJ2020.Managers
{
    public class StageAudioManager : MonoBehaviour
    {
        private AudioSource _audioSource;

        [SerializeField] private AudioClip _pointUp;

        [SerializeField] private AudioClip _beltConvey;

        [SerializeField] private AudioClip _countDown;
        [SerializeField] private AudioClip _gameStart;
        [SerializeField] private AudioClip _gameEnd;

        [SerializeField] private AudioClip _timeAlert;
        [SerializeField] private AudioClip _timeBeep;

        [Inject] private TimeManager _timeManager;

        [SerializeField] private AudioSource _bgm;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();


            _timeManager
                .ReadyTime
                .SkipLatestValueOnSubscribe()
                .Subscribe(x =>
                {
                    if (x > 0)
                    {
                        _audioSource.PlayOneShot(_countDown);
                    }
                    else
                    {
                        _audioSource.PlayOneShot(_gameStart);
                    }
                })
                .AddTo(this);

            _timeManager.RemainingTime
                .SkipLatestValueOnSubscribe()
                .Take(1)
                .Subscribe(_ => _bgm.Play())
                .AddTo(this);

            _timeManager
                .RemainingTime
                .Subscribe(x =>
                {
                    if (x == 30)
                    {
                        PlayTimeAlert();
                    }

                    if (x <= 10 && x > 0)
                    {
                        PlayTimeBeep();
                    }

                    if (x == 0)
                    {
                        _audioSource.PlayOneShot(_gameEnd);
                    }
                }).AddTo(this);
        }

        public void PlayPointUp()
        {
            _audioSource.PlayOneShot(_pointUp);
        }

        public void PlayTimeAlert()
        {
            _audioSource.PlayOneShot(_timeAlert);
        }

        public void PlayTimeBeep()
        {
            _audioSource.PlayOneShot(_timeBeep);
        }

        public void PlayBeltConvey()
        {
            _audioSource.PlayOneShot(_beltConvey);
        }
    }
}