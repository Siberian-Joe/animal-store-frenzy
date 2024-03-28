using System.Collections.Generic;
using Core.Enums;
using Core.ViewModels;
using UniRx;
using UnityEngine;

namespace Core.Views
{
    public class ShelfEntityView : InteractableEntityView<ShelfEntityViewModel>
    {
        [SerializeField] private InteractableEntityType _interactableEntityType;
        [SerializeField] private List<Sprite> _shelfSprites;

        private SpriteRenderer _spriteRenderer;

        protected override void Initialize()
        {
            base.Initialize();

            _spriteRenderer = GetComponent<SpriteRenderer>();
            ViewModel.CurrentCapacity.Subscribe(UpdateShelfSprite).AddTo(Disposable);
            ViewModel.SetInteractableEntityType(_interactableEntityType);
        }

        private void UpdateShelfSprite(int capacity)
        {
            var spriteIndex = Mathf.Clamp(capacity, 0, _shelfSprites.Count - 1);
            _spriteRenderer.sprite = _shelfSprites[spriteIndex];
        }
    }
}