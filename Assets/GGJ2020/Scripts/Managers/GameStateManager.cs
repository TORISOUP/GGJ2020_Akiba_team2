using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace GGJ2020.Managers
{
    public class GameStateManager : MonoBehaviour
    {
        public IReadOnlyReactiveProperty<GameState> CurrentState => _currentState;

        private readonly ReactiveProperty<GameState> _currentState = new ReactiveProperty<GameState>(GameState.READY);

        [Inject] private TimeManager _timeManager;
        [SerializeField] private AudioSource bgm;


        private void Start()
        {
            _timeManager.ReadyTime
                .Where(x => x == 0)
                .Take(1)
                .Subscribe(x => { _currentState.Value = GameState.BATTLE; })
                .AddTo(this);

            _timeManager.RemainingTime
                .Where(x => x == 0)
                .Take(1)
                .Subscribe(x => { _currentState.Value = GameState.FINISHED; });

            _currentState.AddTo(this);
        }
    }
}