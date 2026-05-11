using Game.World.Authoring;
using UnityEngine;

namespace Game.World.Inventory
{
    [CreateAssetMenu(fileName = "ItemDefinition", menuName = "Game/World/Inventory/Item Definition")]
    public sealed class ItemDefinition : StableIdDefinition
    {
        [SerializeField] private string _displayName;
        [SerializeField] private Sprite _icon;

        public ItemId Id => new(StableIdValue);
        public string DisplayName => string.IsNullOrWhiteSpace(_displayName) ? StableIdValue : _displayName;
        public Sprite Icon => _icon;

        protected override void OnValidate()
        {
            base.OnValidate();
            _displayName = _displayName?.Trim();
        }

        public void Validate() => ValidateStableId();
    }
}