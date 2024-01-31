using Core.Enums;
using Interfaces.Interactions;
using Interfaces.Services;
using Interfaces.Services.DataServices;
using Services;
using UniRx;

namespace Core.ViewModels
{
    public class ShelfEntityViewModel : InteractableEntityViewModel<ShelfModel>, IShelfInteraction
    {
        public ReactiveProperty<int> CurrentCapacity => Model.CurrentCapacity;


        public ShelfEntityViewModel(IDataService dataService,
            IInteractableEntitiesLocatorService interactableEntitiesLocatorService) : base(dataService,
            interactableEntitiesLocatorService)
        {
        }

        protected override ShelfModel CreateDefaultModel()
        {
            return new ShelfModel(2);
        }

        public void Interact()
        {
            AddItem(1);
        }

        public void AddItem(int amount)
        {
            Model.AddItem(amount);
        }

        public void RemoveItem(int amount)
        {
            Model.RemoveItem(amount);
        }
    }
}