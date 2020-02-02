using System.Collections.Generic;
using System.Linq;
using GGJ2020.Managers.Scores;
using GGJ2020.Parts;
using UniRx;
using UnityEngine;
using Zenject;

namespace GGJ2020.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        private readonly IntReactiveProperty _totalScore = new IntReactiveProperty();
        public IReadOnlyReactiveProperty<int> TotalScore => _totalScore;


        [Inject] private readonly OrderCalculator _orderCalculator;

        [Inject] private readonly StageManager _stageManager;
        [Inject] private readonly GameStateManager _gameStateManager;
        [Inject] private readonly StageAudioManager _audioManager;

        private void Start()
        {
            _stageManager.ShippingPartsAsObservable
                .TakeUntil(_gameStateManager
                    .CurrentState
                    .FirstOrDefault(x => x == GameState.FINISHED))
                .Subscribe(CalculateScore);

            _totalScore.SkipLatestValueOnSubscribe()
                .DistinctUntilChanged()
                .Subscribe(_ => _audioManager.PlayPointUp());
        }

        private void CalculateScore(ShippingParts shippingParts)
        {
            _totalScore.Value += _orderCalculator.Calculate(shippingParts);
        }
    }
}