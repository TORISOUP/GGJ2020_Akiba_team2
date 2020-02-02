using GGJ2020.Common;
using UniRx;
using UniRx.Async;
using UnityEngine;
using Zenject;

namespace GGJ2020.Managers
{
    public class ResultManager : MonoBehaviour
    {
        [SerializeField] private AudioSource bgm;
        public IReadOnlyReactiveProperty<bool> IsResultShowing => _isResultShowing;
        private readonly ReactiveProperty<bool> _isResultShowing = new ReactiveProperty<bool>(false);

        [Inject] private GameStateManager _gameStateManager;

        private void Start()
        {
            _gameStateManager.CurrentState
                .FirstOrDefault(x => x == GameState.FINISHED)
                .Subscribe(_ => _isResultShowing.Value = true);

            _isResultShowing.AddTo(this);
        }

        public async UniTaskVoid GoToTitleAsync()
        {
            SoundManager.BGMFadeOut(bgm, 2f);
            await UniTask.Delay(2000);
            SceneLoader.LoadScene(GameScenes.TitleScene.ToString(), _ => { });
        }
    }
}