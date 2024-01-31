using System;
using Core.ViewModels;
using UniRx;
using UnityEngine;

namespace Core.Views
{
    public class ChestEntityView : InteractableEntityView<ChestEntityViewModel>
    {
        private readonly int _opened = Animator.StringToHash("Opened");

        private Animator _animator;

        protected override void Initialize()
        {
            _animator = GetComponent<Animator>();

            ViewModel.Opened.Subscribe(UpdateChestAnimation).AddTo(Disposable);
        }

        private void UpdateChestAnimation(bool isOpen)
        {
            _animator.SetBool(_opened, isOpen);
        }

        public override void Interact<TInteraction>(Action<TInteraction> action = null)
        {
            base.Interact(action);

            if (ViewModel.Opened.Value)
            {
                ShowChestItems();
            }
        }

        private void ShowChestItems()
        {
            var items = ViewModel.GetItems();

            foreach (var item in items)
            {
                Debug.Log("Предмет в сундуке: " + item);
            }
        }
    }
}