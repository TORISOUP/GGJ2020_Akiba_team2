using UniRx;
using UniRx.Async;
using UniRx.Triggers;
using UnityEngine;

namespace GGJ2020.Players.InputImpls
{
    public class MultiPlayerInputEventProvider: MonoBehaviour, IInputEventProvider
    {
        private readonly ReactiveProperty<bool> _onActionPushed = new BoolReactiveProperty(false);
        private readonly ReactiveProperty<Vector3> _moveDirection = new ReactiveProperty<Vector3>();
        public IReadOnlyReactiveProperty<bool> ActionButton => _onActionPushed;
        public IReadOnlyReactiveProperty<Vector3> MoveDirection => _moveDirection;

        private  void Start()
        {
            SetUpAsync().Forget();
        }

        private async UniTaskVoid SetUpAsync()
        {
            var core = GetComponent<PlayerCore>();
            
            // wait for initializing of player
            await core.InitializedAsync;

            var id = 1 + (int) core.Id;

            var useButton = "Action" + id;
            var hori = "Horizontal" + id;
            var vert = "Vertical" + id;

            this.UpdateAsObservable()
                .Select(_ => Input.GetButton(useButton))
                .DistinctUntilChanged()
                .Subscribe(x => _onActionPushed.Value = x);

            this.UpdateAsObservable()
                .Select(_ => new Vector3(Input.GetAxis(hori), -Input.GetAxis(vert), 0))
                .Subscribe(x => { _moveDirection.SetValueAndForceNotify(x); });
            _onActionPushed.AddTo(this);
            _moveDirection.AddTo(this);
        }
        
    }
}