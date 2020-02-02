using UniRx;

namespace GGJ2020.Holders
{
    public interface IHoldable
    {
        bool TryHold();
        void Unhold();
        IReadOnlyReactiveProperty<bool> IsHold { get; }
    }
}
