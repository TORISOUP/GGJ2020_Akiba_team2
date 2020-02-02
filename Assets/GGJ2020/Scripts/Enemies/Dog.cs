using GGJ2020.Damages;
using UniRx;
using UniRx.Triggers;
using System.Linq;
using System.Threading;
using UniRx.Async;
using UniRx.Async.Triggers;
using UnityEngine;
using Zenject;

namespace GGJ2020.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class Dog : BaseEnemy
    {
        [Inject] Managers.PlayerManager pm;

        private AudioSource source;

        [SerializeField] private AudioClip attack;
        private Shake shake;
        private Transform[] _players;

        readonly float _dogIsClose = 3f;

        private bool _isLatestHitPlayer = false;

        private Rigidbody2D _rigidbody2D;


        [SerializeField] private Sprite[] _up;
        [SerializeField] private Sprite[] _down;
        [SerializeField] private Sprite[] _right;
        [SerializeField] private Sprite[] _left;

        private SpriteRenderer _spriteRenderer;

        private int _animIndex;

        [SerializeField] private int _animFrame = 3;

        private void Update()
        {
            var velocity = _rigidbody2D.velocity;

            if (Vector2.Dot(velocity, Vector2.up) >= 0.5f)
            {
                _spriteRenderer.sprite = _up[_animIndex];
            }
            else if (Vector2.Dot(velocity, Vector2.down) >= 0.5f)
            {
                _spriteRenderer.sprite = _down[_animIndex];
            }
            else if (Vector2.Dot(velocity, Vector2.right) >= 0.5f)
            {
                _spriteRenderer.sprite = _right[_animIndex];
            }
            else if (Vector2.Dot(velocity, Vector2.left) >= 0.5f)
            {
                _spriteRenderer.sprite = _left[_animIndex];
            }
        }

        private Transform FindClosePlayer()
        {
            return _players
                .OrderByDescending((x) => Vector2.Distance(x.position, transform.position))
                .FirstOrDefault(x => Vector2.Distance(x.position, transform.position) < _dogIsClose);
        }

        private void Start()
        {
            _players = pm.Players.Select(x => x.transform).ToArray();
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _spriteRenderer = GetComponent<SpriteRenderer>();

            shake = Camera.main.GetComponent<Shake>();
            source = GetComponent<AudioSource>();
            source.clip = attack;

            this.OnCollisionEnter2DAsObservable()
                .Select(x => x.gameObject.GetComponent<IDamageApplicable>())
                .Where(x => x != null)
                .Subscribe(x =>
                {
                    _isLatestHitPlayer = true;
                    x.ApplyDamage(new Damage(1.0f));
                    source.Play();
                    shake.ShakeMe(.2f, .1f);
                    _rigidbody2D.velocity = Vector2.zero;
                })
                .AddTo(this);

            LoopAsync(this.GetCancellationTokenOnDestroy()).Forget();

            AnimLoopAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid AnimLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                _animIndex = (_animIndex + 1) % 5;
                await UniTask.DelayFrame(_animFrame, cancellationToken: token);
            }
        }

        private async UniTaskVoid LoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Transform target = null;
                {
                    // Wait for player closing...
                    while (!token.IsCancellationRequested && !_isLatestHitPlayer)
                    {
                        target = FindClosePlayer();
                        if (target != null) break;

                        // Move to random
                        var direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                        _rigidbody2D.velocity = direction;

                        // wait 1..2 seconds.
                        await UniTask.Delay(UnityEngine.Random.Range(1000, 2000), cancellationToken: token);
                    }

                    // pre-charge
                    {
                        if (target != null)
                        {
                            var c = (target.position - transform.position).normalized;
                            _rigidbody2D.velocity = c;
                            await UniTask.Yield();
                            await UniTask.Yield();
                        }

                        _rigidbody2D.velocity = Vector2.zero;
                        await UniTask.Delay(1000, cancellationToken: token);
                    }

                    _isLatestHitPlayer = false;

                    // Charge
                    if (target == null) continue;
                    var chargeDirection = (target.position - transform.position).normalized;
                    _rigidbody2D.velocity = chargeDirection * 5f;
                    await UniTask.Delay(1000, cancellationToken: token);

                    // IgnoreTime
                    for (int i = 0; i < 3; i++)
                    {
                        var direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                        _rigidbody2D.velocity = direction;
                        await UniTask.Delay(UnityEngine.Random.Range(1000, 2000), cancellationToken: token);
                    }
                }
            }
        }
    }
}