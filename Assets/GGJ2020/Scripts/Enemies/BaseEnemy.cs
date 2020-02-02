using GGJ2020.Managers;
using UniRx;
using UnityEngine;
using Zenject;

namespace GGJ2020.Enemies
{
    public class BaseEnemy : MonoBehaviour, Holders.IHoldable
    {
        [SerializeField]
        [Tooltip("Enemy speed")]
        protected float speed = 200;

        private Holders.Holdable holdable;

        public BaseEnemy()
        {
            holdable = new Holders.Holdable();
        }

        [Inject]
        protected GameStateManager GameStateManager;

        public IReadOnlyReactiveProperty<bool> IsHold
            => holdable.IsHold;

        public bool TryHold()
            => holdable.TryHold();

        public void Unhold()
            => holdable.Unhold();
    }
}
