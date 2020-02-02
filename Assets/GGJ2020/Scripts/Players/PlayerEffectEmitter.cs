using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace GGJ2020.Players
{
	public class PlayerEffectEmitter : MonoBehaviour
	{
		public GameObject[] PlayerEffects;

		private PlayerAnimator _playerAnimator;
		private int _playerAnimNo;

		public enum animNo
		{
			Move,
			Putdown,
			PartsAttach,
		}

		private void Start()
		{
			_playerAnimator = GetComponent<PlayerAnimator>();
		}

		public void Effect(int no)
		{
			_playerAnimNo = no;
			StartCoroutine(PlayEffect());
		}

		private IEnumerator PlayEffect()
		{
			var Effect = Instantiate(PlayerEffects[_playerAnimNo]);
			Effect.transform.localScale = Vector3.one;
			Effect.transform.localPosition = GetEffectPos();
			var animator = Effect.GetComponent<Animator>();
			yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
			Destroy(Effect.gameObject);
		}

		private Vector2 GetEffectPos()
		{
			var pos = new Vector2();
			switch(_playerAnimNo)
			{
				case 0:
					{
						pos = MoveEffectPos();
						break;
					}
				case 1:
					{
						pos = PutDown();
						break;
					}
				case 2:
					{
						PartsAttach();
						break;
					}
			}
			return pos;
		}

		private Vector2 MoveEffectPos()
		{
			//Moveができたら位置調整
			Vector2 pos = new Vector2();
			if (_playerAnimator.Direction == Vector2.up)
			{
				pos.y = 0.75f;
			}
			else if (_playerAnimator.Direction == Vector2.right)
			{
				pos.x = 0.5f;
				pos.y = -0.25f;
			}
			else if (_playerAnimator.Direction == Vector2.down)
			{
				pos.y = -1.0f;
			}
			else
			{
				pos.x = -0.5f;
				pos.y = -0.25f;
			}
			return new Vector2(transform.localPosition.x + pos.x, transform.localPosition.y + pos.y);
		}

		private Vector2 PutDown()
		{
			Vector2 pos = new Vector2();
			if (_playerAnimator.Direction == Vector2.up)
			{
				pos.y = 0.75f;
			}
			else if (_playerAnimator.Direction == Vector2.right)
			{
				pos.x = 0.5f;
			}
			else if (_playerAnimator.Direction == Vector2.down)
			{
				pos.y = -0.75f;
			}
			else
			{
				pos.x = -0.5f;
			}
			return new Vector2(transform.localPosition.x + pos.x, transform.localPosition.y + pos.y);
		}

		private Vector2 PartsAttach()
		{
			//TODO:エフェクトが来たら作る
			Vector2 pos = new Vector2();
			return new Vector2(transform.localPosition.x + pos.x, transform.localPosition.y + pos.y);
		}
	}
}
