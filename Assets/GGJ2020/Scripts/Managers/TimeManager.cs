using System;
using System.Threading;
using GGJ2020.Common;
using UniRx;
using UniRx.Async;
using UniRx.Async.Triggers;
using UniRx.Triggers;
using UnityEngine;

namespace GGJ2020.Managers
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private IntReactiveProperty _readyTime = new IntReactiveProperty(3);
        [SerializeField] private IntReactiveProperty _remainingTime;

        public IReadOnlyReactiveProperty<int> ReadyTime => _readyTime;
        public IReadOnlyReactiveProperty<int> RemainingTime => _remainingTime;


        private void Start()
        {
            _readyTime.AddTo(this);
            _remainingTime.AddTo(this);
            CountDownAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid CountDownAsync(CancellationToken token)
        {
            await SceneLoader.OnTransitionFinished;
            await CountReadyTimeAsync(token);
            await CountRemainingTimeAsync(token);
        }

        private async UniTask CountReadyTimeAsync(CancellationToken token)
        {
            await UniTask.Delay(1000, cancellationToken: token);
            _readyTime.SetValueAndForceNotify(_readyTime.Value);
            while (!token.IsCancellationRequested)
            {
                await UniTask.Delay(1000, cancellationToken: token);
                _readyTime.Value--;
                if (_readyTime.Value == 0) return;
            }
        }

        private async UniTask CountRemainingTimeAsync(CancellationToken token)
        {
            await UniTask.Delay(1000, cancellationToken: token);
            _remainingTime.SetValueAndForceNotify(_remainingTime.Value);
            while (!token.IsCancellationRequested)
            {
                await UniTask.Delay(1000, cancellationToken: token);
                _remainingTime.Value--;
                if (_remainingTime.Value == 0) return;
            }
        }
    }
}