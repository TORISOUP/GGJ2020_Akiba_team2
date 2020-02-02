using UniRx;

namespace GGJ2020.Holders
{
    public class Holdable
    {
        public Holdable()
        {
            IsHold = new BoolReactiveProperty(false);
        }

        public bool TryHold()
        {
            if (!IsHold.Value)
            {
                IsHold.Value = true;
                return true;
            }
            return false;
        }

        public void Unhold()
            => IsHold.Value = false;

        public BoolReactiveProperty IsHold { private set; get; }
    }
}
