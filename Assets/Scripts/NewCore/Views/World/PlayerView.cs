using NewCore.ViewModels;
using R3;
using UnityEngine;
using UnityEngine.AI;

namespace NewCore.Views.World
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerView : EntityView<PlayerViewModel>
    {
        private NavMeshAgent _agent;
        private Vector3 _lastPosition;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
        }

        protected override void OnBind()
        {
            base.OnBind();
            ViewModel.TargetPosition
                .Subscribe(targetPosition => _agent.SetDestination(targetPosition))
                .AddTo(Disposables);

            ViewModel.Position
                .Subscribe(position => _agent.Warp(position))
                .AddTo(Disposables);

            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    if (_agent == null || !_agent.enabled)
                        return;

                    if (Vector3.Distance(_lastPosition, _agent.transform.position) > 0.01f)
                    {
                        _lastPosition = _agent.transform.position;
                        ViewModel.Proxy.Position.Value = _lastPosition;
                    }
                })
                .AddTo(Disposables);
        }
    }
}