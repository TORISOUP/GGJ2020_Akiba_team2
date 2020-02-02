using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using GGJ2020.Managers.Scores;
using UniRx;
using UnityEngine;

namespace GGJ2020.Stages
{
    public class ShippingButton : MonoBehaviour, Holders.IHoldable
    {
        [SerializeField] private OrderName _orderName = OrderName.Car;
        private Subject<OrderName> _onPushSubject = new Subject<OrderName>();
        public IObservable<OrderName> OnPushed => _onPushSubject;

        private readonly BoolReactiveProperty _isHolding = new BoolReactiveProperty(false);

        private AudioSource source;

        [SerializeField]
        private Sprite spritePush, spriteNormal;

        private SpriteRenderer sr;

        [SerializeField]
        private float timePressed = .5f;

        private void Start()
        {
            _onPushSubject.AddTo(this);
            source = GetComponent<AudioSource>();
            sr = GetComponent<SpriteRenderer>();
        }

        public IReadOnlyReactiveProperty<bool> IsHold
            => _isHolding; //false fixed

        public bool TryHold()
        {
            _onPushSubject.OnNext(_orderName);
            source.Play();
            sr.sprite = spritePush;
            StartCoroutine("SwitchButtonToNormal");
            return false;
        }

        private IEnumerator SwitchButtonToNormal()
        {
            yield return new WaitForSeconds(timePressed);
            sr.sprite = spriteNormal;
        }

        public void Unhold()
        {
            // do nothing
        }
    }
}