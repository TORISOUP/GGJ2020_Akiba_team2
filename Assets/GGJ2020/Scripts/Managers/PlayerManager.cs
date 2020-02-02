using GGJ2020.Players;
using GGJ2020.Players.InputImpls;
using UniRx;
using UnityEngine;

namespace GGJ2020.Managers
{
    public sealed class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] Player;

        [SerializeField] private Transform[] SpawnPoint;

        public IReadOnlyReactiveCollection<PlayerCore> Players => _players;
        ReactiveCollection<PlayerCore> _players = new ReactiveCollection<PlayerCore>();

        public void InitializePlayer(int playerCount)
        {
            for (int i = 0; i < playerCount; i++)
            {
                // TODO: Position
                var p = Instantiate(Player[i], SpawnPoint[i].position, Quaternion.identity);
                var c = p.GetComponent<PlayerCore>();
                c.Initialize((PlayerId) (i));
                _players.Add(c);
            }
        }
    }
}