using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ2020.Title.View
{
    public class ChoosePlayersButton : MonoBehaviour
    {
        [SerializeField] private int PlayerCount = 1;

        public IObservable<int> OnPushed => _onPushed;
        private Subject<int> _onPushed = new Subject<int>();

        private void Start()
        {
            GetComponent<Button>()
                .OnClickAsObservable()
                .Select(_ => PlayerCount)
                .Subscribe(_onPushed);
        }
    }
}