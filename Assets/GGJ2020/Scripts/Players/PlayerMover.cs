using System;
using GGJ2020.Managers;
using UnityEngine;
using UniRx;
using Zenject;

namespace GGJ2020.Players
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] [Tooltip("Player speed")]
        private float speed = 200f;

        private BoolReactiveProperty _isMoving = new BoolReactiveProperty();
        public IReadOnlyReactiveProperty<bool> IsMoving => _isMoving;

        private ReactiveProperty<Vector2> _currentDirection = new ReactiveProperty<Vector2>();
        public IReadOnlyReactiveProperty<Vector2> CurrentDirection => _currentDirection;

        private PlayerPartHolder _partHolder;
		private PlayerEffectEmitter _playerEffectEmitter;

		private float _EffectSec = 0;

        [Inject] private GameStateManager _gameStateManager;


        private void Start()
        {
            _partHolder = GetComponent<PlayerPartHolder>();
			_playerEffectEmitter = GetComponent<PlayerEffectEmitter>();
			var input = GetComponent<IInputEventProvider>();
            Rigidbody2D rb;
            bool isStunned = false;
            Managers.GameState state = Managers.GameState.READY;
            GetComponent<PlayerCore>().IsStunned.Subscribe(x => { isStunned = x; });
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Managers.GameStateManager>().CurrentState
                .Subscribe(x => { state = x; });
            rb = GetComponent<Rigidbody2D>();

            input.MoveDirection
                .Where(_ => _gameStateManager.CurrentState.Value == GameState.BATTLE)
                .Select(x => x.magnitude > 0.1f ? x.normalized : x)
                .Subscribe(x =>
                {
                    if (!isStunned && state != Managers.GameState.FINISHED)
                    {
                        var remaining = rb.velocity / 5f;
                        rb.velocity = ((Vector2) x * speed) + remaining;
                        _isMoving.Value = rb.velocity != Vector2.zero;
                        if (Math.Abs(rb.velocity.sqrMagnitude) > 0.001f
                        ) // sqrMagnitude is lighter than magnitude, we want to keep track of the last direction the player is looking at
                        {
							_EffectSec += Time.deltaTime;
                            _currentDirection.Value = rb.velocity.normalized;
							//if(_EffectSec > 0.5f)
							//{
							//	_playerEffectEmitter.Effect(1);
							//	_EffectSec = 0;
							//}
							
						}
					}
                    else
                    {
                        rb.velocity /= 3.0f;
                    }
                })
                .AddTo(this);
        }
    }
}