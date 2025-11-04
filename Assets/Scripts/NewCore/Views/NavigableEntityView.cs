using System;
using NewCore.Extensions;
using R3;
using UnityEngine.AI;

namespace NewCore.Views
{
    public abstract class NavigableEntityView<TViewModel> : EntityView<TViewModel>
        where TViewModel : INavigableEntityViewModel
    {
        private NavMeshAgent _agent;
        private IDisposable _arriveDisposable;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
        }

        protected override void OnBind()
        {
            base.OnBind();

            ViewModel.TargetPosition
                .Subscribe(targetPosition =>
                {
                    _agent.SetDestination(targetPosition);

                    _arriveDisposable?.Dispose();
                    _arriveDisposable = _agent
                        .WhenArrived()
                        .Subscribe(_ => ViewModel.NotifyArrived())
                        .AddTo(Disposables);
                })
                .AddTo(Disposables);
        }
    }
}