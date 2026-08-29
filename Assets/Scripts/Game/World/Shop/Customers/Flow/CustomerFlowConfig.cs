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
        [SerializeField] private CustomerTrafficProfile _trafficProfile;
        [SerializeField] private CustomerArchetypeDefinition[] _archetypes;

        public bool AutoStart => _autoStart;
        public string CustomerBlueprintId => _customerBlueprintId;
        public int MaxActiveCustomers => Mathf.Max(1, _maxActiveCustomers);
        public CustomerTrafficProfile TrafficProfile => _trafficProfile;
        public CustomerArchetypeDefinition[] Archetypes => _archetypes;

        public void Validate()
        {
            if (_trafficProfile == false)
            {
                throw new System.InvalidOperationException(
                    $"{nameof(CustomerFlowConfig)} requires a {nameof(CustomerTrafficProfile)} reference.");
            }

            _trafficProfile.Validate();
        }

        private void OnValidate()
        {
            _customerBlueprintId = string.IsNullOrWhiteSpace(_customerBlueprintId)
                ? string.Empty
                : _customerBlueprintId.Trim();
        }
    }
}
