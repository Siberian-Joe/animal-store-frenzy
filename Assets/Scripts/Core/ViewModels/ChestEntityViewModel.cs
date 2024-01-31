using System.Collections.Generic;
using Core.Enums;
using Interfaces.Interactions;
using Interfaces.Services;
using Interfaces.Services.DataServices;
using UniRx;

namespace Core.ViewModels
{
    public class ChestEntityViewModel : InteractableEntityViewModel<ChestModel>, IInteraction
    {
        public ReactiveProperty<bool> Opened => Model.Opened;

        public ChestEntityViewModel(IDataService dataService,
            IInteractableEntitiesLocatorService interactableEntitiesLocatorService) : base(dataService,
            interactableEntitiesLocatorService)
        {
        }

        public IReadOnlyList<string> GetItems()
        {
            return Model.Items.AsReadOnly();
        }

        public void Interact()
        {
            if (Model.Opened.Value)
                Close();
            else
                Open();
        }

        protected override ChestModel CreateDefaultModel()
        {
            return new ChestModel();
        }

        private void Open()
        {
            Model.Opened.Value = true;
        }

        private void Close()
        {
            Model.Opened.Value = false;
        }
    }
}