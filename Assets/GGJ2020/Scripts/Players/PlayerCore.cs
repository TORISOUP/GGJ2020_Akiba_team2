using UniRx;
using UniRx.Async;
using UnityEngine;

namespace GGJ2020.Players
{
    public class PlayerCore : MonoBehaviour, Damages.IDamageApplicable
    {
        public PlayerId Id { get; private set; }
        
        public UniTask InitializedAsync => _uniTaskCompletionSource.Task;
        private readonly UniTaskCompletionSource _uniTaskCompletionSource = new UniTaskCompletionSource();

        private BoolReactiveProperty isStunned = new BoolReactiveProperty();

        // Please call here by Manager
        public void Initialize(PlayerId id)
        {
            Id = id;
            _uniTaskCompletionSource.TrySetResult();
        }

        private void OnDestroy()
        {
            _uniTaskCompletionSource.TrySetCanceled();
        }

        public IReadOnlyReactiveProperty<bool> IsStunned
        {
            get { return isStunned; }
        }

        public void ApplyDamage(Damages.Damage damage)
        {
            isStunned.Value = true;
            
            //TODO: Fix it, it cannot work in call many times
            _ = UniTask.Delay((int)(damage.StunSeconds * 1000)).ContinueWith(() =>
            {
                isStunned.Value = false;
            });
        }
    }
}