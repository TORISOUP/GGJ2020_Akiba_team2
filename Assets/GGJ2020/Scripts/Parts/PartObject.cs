using System;
using GGJ2020.Managers.Scores;
using GGJ2020.Stages;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GGJ2020.Parts
{
    public class PartObject : MonoBehaviour, Holders.IHoldable
    {
        [SerializeField] private Part _part;
        public Part Part => _part;

        private bool _isInAssemblyArea = false;

        private void Start()
        {
            
            holdable = new Holders.Holdable();

            this.OnTriggerEnter2DAsObservable()
                .Where(x => x.transform.parent != null)
                .Select(x => x.transform.parent.GetComponent<AssemblyArea>())
                .Subscribe(x =>
                {
                    if (x != null) x.AddPart(this);
                })
                .AddTo(this);

            this.OnTriggerExit2DAsObservable()
                .Where(x => x.transform.parent != null)
                .Select(x => x.transform.parent.GetComponent<AssemblyArea>())
                .Subscribe(x =>
                {
                    if (x != null) x.RemovePart(this);
                })
                .AddTo(this);
        }

        public bool TryHold()
            => holdable.TryHold();

        public void Unhold()
            => holdable.Unhold();

        private Holders.Holdable holdable;

        public IReadOnlyReactiveProperty<bool> IsHold
            => holdable.IsHold;
    }
}