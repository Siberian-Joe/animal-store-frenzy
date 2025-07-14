using System;
using NewCore.Components;
using NewCore.ViewModels;
using R3;
using UnityEngine;

namespace NewCore.Views
{
    [RequireComponent(typeof(EntityIdentifier))]
    public abstract class EntityView<TViewModel> : View<TViewModel> where TViewModel : IEntityViewModel
    {
        private const float PositionToleranceSqr = 0.0001f;
        private const float PositionUpdateInterval = 200f;

        public string ID => ViewModel?.ID ?? (_identifier ??= GetComponent<EntityIdentifier>()).ID;

        private EntityIdentifier _identifier;

        protected override void OnBind()
        {
            base.OnBind();

            if (_identifier == null)
                _identifier = GetComponent<EntityIdentifier>();

            _identifier.ID = ViewModel.ID;

            if (ViewModel != null)
            {
                transform.position = ViewModel.Position.Value;

                Observable.EveryUpdate()
                    .Select(_ => transform.position)
                    .DistinctUntilChanged()
                    .ThrottleFirst(TimeSpan.FromMilliseconds(PositionUpdateInterval))
                    .Subscribe(positionValue =>
                    {
                        if ((ViewModel.Position.Value - positionValue).sqrMagnitude > PositionToleranceSqr)
                            ViewModel.Position.Value = positionValue;
                    })
                    .AddTo(Disposables);
            }
        }
    }
}