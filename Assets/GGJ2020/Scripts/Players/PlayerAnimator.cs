using UnityEngine;
using UnityEngine.UI;

namespace GGJ2020.Players
{
	public class PlayerAnimator : MonoBehaviour
	{
		public Vector2 Direction { get { return direction; } }

		public Sprite[] PlayerSprits;

		private PlayerMover _playerMover;
		private PlayerCore _playerCore;
		private Transform _Player;
		private SpriteRenderer _PlayerSpriteRenderer;
		private Animator playerAnimator;
		private float alpha;
		private float animationtime;

		private Vector2 direction = Vector2.up;

		void Start()
		{
			_playerMover = GetComponent<PlayerMover>();
			_playerCore = GetComponent<PlayerCore>();
			playerAnimator = GetComponent<Animator>();
			_PlayerSpriteRenderer = this.transform.GetComponent<SpriteRenderer>();
			animationtime = 0f;
			alpha = 1f;
			SpriteChange();
		}

		void Update()
		{

			StunAnimation();
			if(_playerMover.IsMoving.Value == true)
			{
				SpriteChange();
			}
			else
			{
				playerAnimator.SetBool("Up", false);
				playerAnimator.SetBool("Down", false);
				playerAnimator.SetBool("Left", false);
				playerAnimator.SetBool("Right", false);
			}
			
		}


		private void SpriteChange()
		{
			//var dir = 0;
			// angle仮置き
			var angle = GetAngle(this.transform.localPosition, new Vector2(transform.localPosition.x + _playerMover.CurrentDirection.Value.x, transform.localPosition.y + _playerMover.CurrentDirection.Value.y));

			if(-80.0f < angle && angle < 80.0f)
			{
				//dir = 0;
				playerAnimator.SetBool("Up", true);
				playerAnimator.SetBool("Down", false);
				playerAnimator.SetBool("Left", false);
				playerAnimator.SetBool("Right", false);
				direction = Vector2.up;
			}
			else if(80.0f < angle && angle < 160.0f)
			{
				//dir = 1;
				playerAnimator.SetBool("Up", false);
				playerAnimator.SetBool("Down", false);
				playerAnimator.SetBool("Left", false);
				playerAnimator.SetBool("Right", true);
				direction = Vector2.right;
			}
			else if (160.0f < angle && angle < 240.0f)
			{
				//dir = 2;
				playerAnimator.SetBool("Up", false);
				playerAnimator.SetBool("Down", true);
				playerAnimator.SetBool("Left", false);
				playerAnimator.SetBool("Right", false);
				direction = Vector2.down;
			}
			else
			{
				//dir = 3;
				playerAnimator.SetBool("Up", false);
				playerAnimator.SetBool("Down", false);
				playerAnimator.SetBool("Left", true);
				playerAnimator.SetBool("Right", false);
				direction = Vector2.left;
			}

			//_PlayerSpriteRenderer.sprite = PlayerSprits[dir];
		}

		float GetAngle(Vector2 start, Vector2 target)
		{
			Vector2 dt = target - start;
			float rad = Mathf.Atan2(dt.x, dt.y);
			float degree = rad * Mathf.Rad2Deg;

			if (degree < 0)
			{
				degree += 360;
			}

			return degree;
		}

		private void HoldAnimation()
		{

		}

		private void StunAnimation()
		{
			if (_playerCore.IsStunned.Value)
			{
				animationtime += Time.deltaTime;
				if (animationtime < 0.3f)
				{
					alpha = 1;
				}
				else if (animationtime < 0.5f)
				{
					alpha = 0;
				}
				else
				{
					animationtime = 0;
				}
			}
			else
			{
				alpha = 1;
				animationtime = 0;
			}
			
			var color = _PlayerSpriteRenderer.color;
			color.a = alpha;
			_PlayerSpriteRenderer.color = color;
		}
	}
}

