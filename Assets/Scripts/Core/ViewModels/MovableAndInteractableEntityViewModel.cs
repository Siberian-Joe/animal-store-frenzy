using System;
using Core.Enums;
using Core.Models;
using Interfaces.Core;
using Interfaces.Interactions;
using Interfaces.Services;
using Interfaces.Services.DataServices;
using UniRx;

namespace Core.ViewModels
{
    public abstract class MovableAndInteractableEntityViewModel<TModel> : MovableEntityViewModel<TModel>,
        IInteractableEntityViewModel
        where TModel : MovableAndInteractableEntityModel
    {
        public IReadOnlyReactiveProperty<InteractableEntityType> InteractableEntityType => Model.InteractableEntityType;

        protected readonly IInteractableEntitiesLocatorService InteractableEntitiesLocatorService;


        protected MovableAndInteractableEntityViewModel(IDataService dataService, ILoggingService loggingService,
            IInteractableEntitiesLocatorService interactableEntitiesLocatorService) : base(dataService, loggingService)
        {
            InteractableEntitiesLocatorService = interactableEntitiesLocatorService;
        }

        public override void Initialize()
        {
            base.Initialize();

            Model.InteractableEntityType
                .Subscribe(_ => { InteractableEntitiesLocatorService.RegisterEntity(this); })
                .AddTo(Disposable);
        }

        public virtual void Interact<TInteraction>(Action<TInteraction> action = null) where TInteraction : IInteraction
        {
            if (this is TInteraction interaction)
                action?.Invoke(interaction);
        }

        public void SetInteractableEntityType(InteractableEntityType interactableEntityType)
        {
            Model.InteractableEntityType.Value = interactableEntityType;
        }
    }
}