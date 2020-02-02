using GGJ2020.Parts;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GGJ2020.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class Roomba : BaseEnemy
    {
        private Rigidbody2D rb;
        private Vector2 direction;

        private AudioSource source;

        [SerializeField] private AudioClip attack;

        [SerializeField] private Sprite _up;
        [SerializeField] private Sprite _down;
        [SerializeField] private Sprite _right;
        [SerializeField] private Sprite _left;

        private SpriteRenderer _spriteRenderer;

        
        private void Update()
        {
            var velocity = rb.velocity;
            
            if (Vector2.Dot(velocity, Vector2.up) >= 0.5f)
            {
                _spriteRenderer.sprite = _up;
            }
            else if (Vector2.Dot(velocity, Vector2.down) >= 0.5f)
            {
                _spriteRenderer.sprite = _down;
            }
            else if (Vector2.Dot(velocity, Vector2.right) >= 0.5f)
            {
                _spriteRenderer.sprite = _right;
            }
            else if (Vector2.Dot(velocity, Vector2.left) >= 0.5f)
            {
                _spriteRenderer.sprite = _left;
            }
        }
        
        private void Start()
        {
            source = GetComponent<AudioSource>();
            source.clip = attack;
            rb = GetComponent<Rigidbody2D>();
            direction = Vector2.one;

            _spriteRenderer = GetComponent<SpriteRenderer>();

            this.FixedUpdateAsObservable()
                .Where(_ => GameStateManager.CurrentState.Value ==
                            Managers.GameState.BATTLE && rb.bodyType == RigidbodyType2D.Dynamic)
                .Subscribe(_ => { rb.velocity = direction * speed; });

            this.OnCollisionEnter2DAsObservable()
                .Subscribe(x =>
                {
                    if (base.IsHold.Value) return;
                    var p = x.gameObject.GetComponent<PartObject>();
                    if (p != null && !p.IsHold.Value)
                    {
                        source.Play();
                        Destroy(x.gameObject);
                    }
                    else if (x.collider.CompareTag("WallVer"))
                    {
                        direction = new Vector2(direction.x * -1f, direction.y);
                    }
                    else if (x.collider.CompareTag("WallHor"))
                    {
                        direction = new Vector2(direction.x, direction.y * -1f);
                    }
                });
        }

    }
}