using System;
using Core.Enums;
using Interfaces.Interactions;
using Interfaces.Services;
using Interfaces.Services.DataServices;
using UniRx;

namespace Core.ViewModels
{
    public abstract class InteractableEntityViewModel<TModel> : EntityViewModel<TModel>, IInteractableEntityViewModel
        where TModel : InteractableEntityModel
    {
        protected readonly IInteractableEntitiesLocatorService InteractableEntitiesLocatorService;

        public IReadOnlyReactiveProperty<InteractableEntityType> InteractableEntityType => Model.InteractableEntityType;

        protected InteractableEntityViewModel(IDataService dataService,
            IInteractableEntitiesLocatorService interactableEntitiesLocatorService) : base(dataService)
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

        public override void Dispose()
        {
            base.Dispose();

            InteractableEntitiesLocatorService.UnregisterEntity(this);
        }
    }
}