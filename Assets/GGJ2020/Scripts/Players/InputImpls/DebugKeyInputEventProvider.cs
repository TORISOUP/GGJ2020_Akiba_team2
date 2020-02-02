using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GGJ2020.Players.InputImpls
{
    public class DebugKeyInputEventProvider : MonoBehaviour, IInputEventProvider
    {
        private readonly ReactiveProperty<bool> _onActionPushed = new BoolReactiveProperty(false);
        private readonly ReactiveProperty<Vector3> _moveDirection = new ReactiveProperty<Vector3>();
        public IReadOnlyReactiveProperty<bool> ActionButton => _onActionPushed;
        public IReadOnlyReactiveProperty<Vector3> MoveDirection => _moveDirection;

        private void Start()
        {
            this.UpdateAsObservable()
                .Select(_ => Input.GetKey(KeyCode.Z))
                .DistinctUntilChanged()
                .Subscribe(x => _onActionPushed.Value = x);

            this.UpdateAsObservable()
                .Select(_ =>
                    new Vector3(Input.GetAxisRaw("Horizontal1"), Input.GetAxisRaw("Vertical1"), 0))
                .Subscribe(x => _moveDirection.SetValueAndForceNotify(x));

            _onActionPushed.AddTo(this);
            _moveDirection.AddTo(this);
        }
    }
}