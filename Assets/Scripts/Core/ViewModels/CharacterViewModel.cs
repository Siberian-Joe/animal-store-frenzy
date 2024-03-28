using Core.Models;
using Interfaces.Services;
using Interfaces.Services.DataServices;
using UniRx;
using UnityEngine;

namespace Core.ViewModels
{
    public class CharacterViewModel : MovableEntityViewModel<CharacterModel>
    {
        public ReactiveCommand Interact { get; } = new();

        public CharacterViewModel(IDataService dataService, ILoggingService loggingService, IInputService inputService)
            : base(dataService, loggingService)
        {
            inputService.Direction
                .Subscribe(UpdateDirection)
                .AddTo(Disposable);

            inputService.Interact
                .Subscribe(_ => Interact.Execute())
                .AddTo(Disposable);

            Direction.Subscribe(direction => { Model.TargetPosition.Value = direction; }).AddTo(Disposable);

            DistanceThreshold = 0f;
            IsMovingToDirection = true;
        }

        //TODO: Extract to a common implementation for automatic determination of the type of walking animation
        public int CalculateDirectionIndex(Vector2 direction)
        {
            return direction.x < 0 ? 3 : direction.x > 0 ? 2 : direction.y > 0 ? 1 : 0;
        }

        protected override CharacterModel CreateDefaultModel()
        {
            return new CharacterModel();
        }
    }
}