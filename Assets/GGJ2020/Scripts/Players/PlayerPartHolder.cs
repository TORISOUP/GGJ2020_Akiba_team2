using GGJ2020.Holders;
using UniRx;
using UnityEngine;
using System.Collections.Generic;

namespace GGJ2020.Players
{
    public class PlayerPartHolder : MonoBehaviour
    {
        public ReactiveProperty<IHoldable> CurrentHold { private set; get; }
        private GameObject holdGo;

        [SerializeField] [Tooltip("Player's head")]
        private Transform head;

        private PlayerMover playerMover;

        [SerializeField]
        private AudioClip clipTake, clipPut;

        private AudioSource source;

        [Header("手の半径")][SerializeField]private  float
            sphereCastRadius; // Radius of the sphere cast used to detect if the player can hold smth or 
        [Header("手の長さ")] [SerializeField] private float sphereCastDistance;
        private int ignorePlayerLayer; // All layer except the one the player is in

        private const float
            minDistPick = .1f; // Min distance at which the player can grab. Is added to sphereCastRadius

        public IReadOnlyReactiveProperty<bool> IsHold => _isHold;
        private readonly BoolReactiveProperty _isHold = new BoolReactiveProperty();

        private PlayerAnimator playerAnimator;

		private PlayerEffectEmitter _playerEffectEmitter;

        private void Awake()
        {
            playerAnimator = GetComponent<PlayerAnimator>();
        }

        private void Start()
        {
            source = GetComponent<AudioSource>();
			_playerEffectEmitter = GetComponent<PlayerEffectEmitter>();
			ignorePlayerLayer = ~(1 << LayerMask.NameToLayer("Player"));
            playerMover = GetComponent<PlayerMover>();
            CurrentHold = new ReactiveProperty<IHoldable>(null);
            var input = GetComponent<IInputEventProvider>();

            input.ActionButton
                .Where(x => x)
                .Subscribe(x =>
                {
                    if (CurrentHold.Value != null)
                    {
                        CurrentHold.Value.Unhold();
                        holdGo.transform.parent = null;
                        holdGo.GetComponent<Collider2D>().enabled = true;
                        var tmpRb = holdGo.GetComponent<Rigidbody2D>();
                        if (tmpRb != null)
                        {
                            holdGo.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                        }

                        CurrentHold.Value = null;
                        float xPos = playerMover.CurrentDirection.Value.x;
                        float yPos = playerMover.CurrentDirection.Value.y;
                        if (Mathf.Abs(xPos) > Mathf.Abs(yPos))
                            holdGo.transform.position =
                                (Vector2) transform.position + new Vector2(xPos > 0f ? .5f : -.5f, 0f);
                        else
                            holdGo.transform.position =
                                (Vector2) transform.position + new Vector2(0f, yPos > 0f ? .8f : -.8f);
                        source.clip = clipPut;
                        source.Play();
						_playerEffectEmitter.Effect(0);

					}
                    else
                    {
                        var playerDirection = new Vector3(playerAnimator.Direction.x, playerAnimator.Direction.y, 0) * sphereCastDistance;
                        var casts = Physics2D.CircleCastAll(transform.position + playerDirection, sphereCastRadius,
                            (Vector2) transform.position + playerMover.CurrentDirection.Value, minDistPick,
                            ignorePlayerLayer);
                        var castList = new List<RaycastHit2D>();
                        castList.AddRange(casts);
                        castList.Sort((a, b) => (int)Vector2.Distance(a.transform.position, transform.position)
                        - (int)Vector2.Distance(b.transform.position, transform.position));
                        var targetCastNum = 0;
                        var hit = false;
                        for(int i = 0; i < castList.Count; i++)
                        {
                            if (castList[i].transform.GetComponent<IHoldable>() != null)
                            {
                                targetCastNum = i;
                                hit = true;
                                break;
                            }
                        }
                        if (hit)
                        {
                            var holdable = castList[targetCastNum].transform.GetComponent<IHoldable>();
                            var result = holdable.TryHold();
                            if (!result) return;
                            castList[targetCastNum].collider.transform.parent = head;
                            castList[targetCastNum].collider.GetComponent<Collider2D>().enabled = false;
                            var tmpRb = castList[targetCastNum].collider.GetComponent<Rigidbody2D>();
                            if (tmpRb != null)
                            {
                                tmpRb.velocity = Vector2.zero;
                                tmpRb.bodyType = RigidbodyType2D.Kinematic;
                            }

                            castList[targetCastNum].transform.position = head.position;
                            CurrentHold.Value = holdable;
                            holdGo = castList[targetCastNum].transform.gameObject;
                            source.clip = clipTake;
                            source.Play();
                        }
                        /*
                        if (cast)
                        {
                            Debug.Log("cast" + cast.collider.gameObject.name);
                            var holdable = cast.transform.GetComponent<IHoldable>();
                            if (holdable != null)
                            {
                                var result = holdable.TryHold();
                                if (!result) return;
                                cast.collider.transform.parent = head;
                                cast.collider.GetComponent<Collider2D>().enabled = false;
                                var tmpRb = cast.collider.GetComponent<Rigidbody2D>();
                                if (tmpRb != null)
                                {
                                    tmpRb.velocity = Vector2.zero;
                                    tmpRb.bodyType = RigidbodyType2D.Kinematic;
                                }

                                cast.transform.position = head.position;
                                CurrentHold.Value = holdable;
                                holdGo = cast.transform.gameObject;
                                source.clip = clipTake;
                                source.Play();
                            }
                        }
                        */
                    }
                })
                .AddTo(this);
        }
        /*
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (playerAnimator == null) return;
            var playerDirection = new Vector3(playerAnimator.Direction.x, playerAnimator.Direction.y, 0) * sphereCastDistance;
            //Debug.Log(playerDirection);
            Gizmos.DrawSphere(transform.position + playerDirection, sphereCastRadius);
        }
        */
    }
}