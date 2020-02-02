using System;
using GGJ2020.Common;
using GGJ2020.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GGJ2020.Title.Manager
{
    public class TitleManager : MonoBehaviour
    {
		[SerializeField] private GameObject IntroCanvas;
		[SerializeField] private EventSystem _eventsystem;
		[SerializeField] private Button _button;
		[SerializeField] private AudioSource bgm;
        [SerializeField] private float bgmFadeOutTime;
		private int _playerCount;

        private void Start()
        {
            Cursor.visible = true;
			bgm.Play();
        }

		public void DispIntroduce(int playerCount)
		{
			_playerCount = playerCount;
			IntroCanvas.SetActive(true);
			EventSystem.current.SetSelectedGameObject(_button.gameObject);
		}

        public void GotoStageScene()
        {
            SoundManager.BGMFadeOut(bgm, 0.5f);
            SceneLoader.LoadScene(GameScenes.StageScene.ToString(), container =>
            {
                container.Bind<GameStartOption>()
                    .FromInstance(new GameStartOption(_playerCount));
            });
        }
    }
}