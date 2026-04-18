using UnityEngine;

namespace Game.World.Shop.Customers.Flow
{
    [CreateAssetMenu(
        fileName = "CustomerFlowConfig",
        menuName = "Game/Shop/Customer Flow Config")]
    public sealed class CustomerFlowConfig : ScriptableObject
    {
        [SerializeField] private bool _autoStart = true;
        [SerializeField] private string _customerBlueprintId;
        [SerializeField, Min(1)] private int _maxActiveCustomers = 4;
        [SerializeField, Min(0.1f)] private float _minSpawnInterval = 3f;
        [SerializeField, Min(0.1f)] private float _maxSpawnInterval = 6f;
        [SerializeField] private CustomerArchetypeDefinition[] _archetypes;

        public bool AutoStart => _autoStart;
        public string CustomerBlueprintId => _customerBlueprintId;
        public int MaxActiveCustomers => Mathf.Max(1, _maxActiveCustomers);
        public CustomerArchetypeDefinition[] Archetypes => _archetypes;

        public float GetRandomSpawnInterval()
        {
            var min = Mathf.Max(0.1f, _minSpawnInterval);
            var max = Mathf.Max(min, _maxSpawnInterval);
            return Random.Range(min, max);
        }

        private void OnValidate()
        {
            _customerBlueprintId = string.IsNullOrWhiteSpace(_customerBlueprintId)
                ? string.Empty
                : _customerBlueprintId.Trim();

            if (_maxSpawnInterval < _minSpawnInterval)
                _maxSpawnInterval = _minSpawnInterval;
        }
    }
}