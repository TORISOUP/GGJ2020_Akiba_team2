using System;
using GGJ2020.Managers.Scores;
using GGJ2020.Stages;
using UniRx;
using UniRx.Async;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using UniRx.Async;
using UniRx.Async.Triggers;
using Zenject;

namespace GGJ2020.Managers
{
    public class StageManager : MonoBehaviour
    {
        [Header("パーツセットが中央まで移動するにかかる時間")] [SerializeField]
        private float partsMoveToCenterTime;

        [Header("パーツセットが納品にかかる時間")] [SerializeField]
        private float partsShippingTime;

        [Header("パーツセットの納品完了から新しいパーツセットが出現するまでの間隔")] [SerializeField]
        private int waitForNewPartsMillisecond;

        [SerializeField] private ShippingButton[] _shippingButtons;
        [SerializeField] private GameObject assemblyAreaPrefab;

        [SerializeField] private GGJ2020.Stages.BeltConveyor _beltConveyor;

        private AssemblyArea _assemblyArea;

        [Inject] private StageAudioManager _audioManager;
        
        /// <summary>
        /// 出荷されたPartリストを外部に通知する
        /// </summary>
        public IObservable<ShippingParts> ShippingPartsAsObservable
            => _shippingPartSubject;

        private Subject<ShippingParts> _shippingPartSubject = new Subject<ShippingParts>();

        [Inject] private GameStateManager _gameStateManager;

        void Start()
        {
            _shippingPartSubject.AddTo(this);
            _gameStateManager.CurrentState
                .FirstOrDefault(x => x == GameState.BATTLE)
                .Subscribe(_ =>
                    CreateLoopAsync(this.GetCancellationTokenOnDestroy()).Forget());
        }

        private async UniTaskVoid CreateLoopAsync(CancellationToken token)
        {
            var shipEvent = _shippingButtons
                .Select(x => x.OnPushed)
                .Merge()
                .Publish();
            shipEvent.Connect().AddTo(this);

            // 開始直後にちょっと待つ
            await UniTask.Delay(1000, cancellationToken: token);

            while (!token.IsCancellationRequested)
            {
                // 新しいAssemblyAreaを生成して配置
                await CreateNewAssemblyAreaAsync();

                // 出荷ボタンが押されるのをまつ
                var orderName = await shipEvent.ToUniTask(useFirstValue: true,
                    cancellationToken: token);

                // 出荷する
                var shippingParts = await WaitForShippingAsync(orderName);
                
                // 出荷したPartを通知
                _shippingPartSubject.OnNext(shippingParts);

                await UniTask.Delay(waitForNewPartsMillisecond, cancellationToken: token);
            }
        }


        private async UniTask<ShippingParts> WaitForShippingAsync(OrderName orderName)
        {
            await _beltConveyor.MoveToShippingAsync(partsShippingTime);

            var currentParts = _assemblyArea
                .CurrentPartObjects
                .Select(x => x.Part)
                .ToArray();

            _assemblyArea.DestroyAllParts();
            Destroy(_assemblyArea.gameObject);

            return new ShippingParts(currentParts, orderName);
        }

        private async UniTask CreateNewAssemblyAreaAsync()
        {
            var assemblyAreaObj =
                Instantiate<GameObject>(
                    assemblyAreaPrefab,
                    _beltConveyor.PartsAppPoint,
                    Quaternion.identity);

            _assemblyArea = assemblyAreaObj.GetComponent<AssemblyArea>();
            _assemblyArea.Init();

            _beltConveyor.SetFieldAssemblyArea(assemblyAreaObj.GetComponent<AssemblyArea>());
            await _beltConveyor.MoveToCenterAsync(partsMoveToCenterTime);
        }
    }
}