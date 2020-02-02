using UniRx;
using UnityEngine;

namespace GGJ2020.Players
{
    public interface IInputEventProvider
    {
        IReadOnlyReactiveProperty<bool> ActionButton { get; }
        IReadOnlyReactiveProperty<Vector3> MoveDirection { get; }
    }
}