using System;
using GGJ2020.Common;
using GGJ2020.Managers;
using TMPro;
using UnityEngine;
using Zenject;
using UniRx;

namespace GGJ2020.UIs
{
    public class StageUIPresenter : MonoBehaviour
    {
        [Inject] private ScoreManager _scoreManager;
        [Inject] private TimeManager _timeManager;

        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _readyTime;
        [SerializeField] private TextMeshProUGUI _mainTime;

        void Start()
        {
            var mainTimerShaker = _mainTime.GetComponent<ElementShake>();
            var readyTimerShaker = _readyTime.GetComponent<ElementShake>();

            _mainTime.text = "";

            _scoreManager.TotalScore
                .Subscribe(x => _scoreText.text = $"{x}")
                .AddTo(this);

            _timeManager.ReadyTime
                .Subscribe(x =>
                {
                    if (x == 0)
                    {
                        _readyTime.text = "GO";
                        readyTimerShaker.ShakePosition(100, 30);
                        readyTimerShaker.ShakeRotation(50, 30);

                        _mainTime.text = _timeManager.RemainingTime.Value.ToString();
                    }
                    else
                    {
                        _readyTime.text = $"{x}";
                        _readyTime.fontSize += 50;
                    }
                }).AddTo(this);

            _timeManager.RemainingTime
                .SkipLatestValueOnSubscribe()
                .Subscribe(x =>
                {
                    _readyTime.text = "";

                    if (x == 0)
                    {
                        _mainTime.text = "";
                    }
                    else
                    {
                        _mainTime.text = $"{x}";
                    }

                    if (x == 10)
                    {
                        _mainTime.color = Color.red;
                    }

                    if (x <= 10 && x > 0)
                    {
                        mainTimerShaker.ShakePosition(5, 5);
                        mainTimerShaker.ShakeRotation(5, 5);
                        _mainTime.fontSize += 10;
                    }
                }).AddTo(this);
        }
    }
}