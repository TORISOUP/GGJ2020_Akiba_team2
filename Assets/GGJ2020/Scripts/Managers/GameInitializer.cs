using GGJ2020.Common;
using GGJ2020.Players;
using UniRx.Async;
using UnityEngine;
using Zenject;

namespace GGJ2020.Managers
{
    public class GameInitializer : MonoBehaviour
    {
        //[SerializeField] private GameObject Dog;
        //[SerializeField] private GameObject Roomba;

        [Inject] private PlayerManager _playerManager;

        [InjectOptional] private GameStartOption _gameStartOption;

        void Start()
        {
            Cursor.visible = false;
            SetupAsync().Forget();
        }

        private async UniTaskVoid SetupAsync()
        {
            if (_gameStartOption != null)
            {
                _playerManager.InitializePlayer(_gameStartOption.PlayerCount);
            }
            else
            {
                _playerManager.InitializePlayer(1);
            }

            //Instantiate(Dog, new Vector2(-2f, -2f), Quaternion.identity);
            //Instantiate(Roomba, new Vector2(4f, -1f), Quaternion.identity);
        }
    }
}