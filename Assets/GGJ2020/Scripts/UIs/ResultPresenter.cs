using GGJ2020.Common;
using GGJ2020.Managers;
using TMPro;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace GGJ2020.UIs
{
    public class ResultPresenter : MonoBehaviour
    {
        [Inject] private ResultManager _resultManager;

        [SerializeField] private GameObject _canvas;
        [SerializeField] private TextMeshProUGUI _youEarned;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [Inject] private ScoreManager _scoreManager;

        [SerializeField] private float fade1 = 1.0f;
        [SerializeField] private int waitMillSeconds = 1000;
        [SerializeField] private float fade2 = 1.0f;

        void Start()
        {
            _canvas.SetActive(false);
            _resultManager.IsResultShowing
                .Where(x => x)
                .Take(1)
                .Subscribe(_ => { ShowResultAsync().Forget(); }).AddTo(this);
        }

        private async UniTaskVoid ShowResultAsync()
        {
            _canvas.SetActive(true);
            _scoreText.color = Color.white.SetA(0);
            _youEarned.color = Color.white.SetA(0);

            await UniTask.Delay(800);
            
            var process1 = 0.0f;
            while (process1 < 1.0f)
            {
                process1 += (Time.deltaTime / fade1);
                _youEarned.color = _youEarned.color.SetA(Mathf.Lerp(0.0f, 1.0f, process1));
                await UniTask.Yield();
            }

            await UniTask.Delay(waitMillSeconds);
            _scoreText.text = $"{_scoreManager.TotalScore.Value}";

            var process2 = 0.0f;
            while (process2 < 1.0f)
            {
                process2 += (Time.deltaTime / fade2);
                _scoreText.color = _scoreText.color.SetA(Mathf.Lerp(0.0f, 1.0f, process2));
                await UniTask.Yield();
            }


            await UniTask.Delay(3000);
            _resultManager.GoToTitleAsync().Forget();
        }
    }
}