using System.Linq;
using GGJ2020.Title.Manager;
using GGJ2020.Title.View;
using UniRx;
using UnityEngine;
using Zenject;

namespace GGJ2020.Title.Presenter
{
    public class ChoosePlayerPresenter : MonoBehaviour
    {
        [SerializeField] private ChoosePlayersButton[] _buttons;

        [Inject] private TitleManager _titleManager;

        void Start()
        {
            _buttons.Select(x => x.OnPushed)
                .Merge()
                .Take(1)
                .Subscribe(x => { _titleManager.DispIntroduce(x); }).AddTo(this);
        }
    }
}