using UnityEngine;

namespace Game.World.Shop.Customers.Flow
{
    [CreateAssetMenu(
        fileName = "CustomerArchetypeDefinition",
        menuName = "Game/Shop/Customer Archetype Definition")]
    public sealed class CustomerArchetypeDefinition : ScriptableObject
    {
        [SerializeField] private string _archetypeId;
        [SerializeField, Min(1)] private int _spawnWeight = 1;
        [SerializeField, Min(1)] private int _minInitialNeedCount = 1;
        [SerializeField, Min(1)] private int _maxInitialNeedCount = 2;
        [SerializeField] private CustomerNeedProfileEntry[] _needProfiles;

        public string ArchetypeId => _archetypeId;
        public int SpawnWeight => Mathf.Max(1, _spawnWeight);
        public int MinInitialNeedCount => Mathf.Max(1, _minInitialNeedCount);
        public int MaxInitialNeedCount => Mathf.Max(MinInitialNeedCount, _maxInitialNeedCount);
        public CustomerNeedProfileEntry[] NeedProfiles => _needProfiles;

        public int GetRandomInitialNeedCount() =>
            Random.Range(MinInitialNeedCount, MaxInitialNeedCount + 1);

        private void OnValidate()
        {
            _archetypeId = string.IsNullOrWhiteSpace(_archetypeId)
                ? string.Empty
                : _archetypeId.Trim();

            if (_maxInitialNeedCount < _minInitialNeedCount)
                _maxInitialNeedCount = _minInitialNeedCount;

            if (_needProfiles == null)
                return;

            for (var index = 0; index < _needProfiles.Length; index++)
            {
                var entry = _needProfiles[index];

                entry.NeedId = string.IsNullOrWhiteSpace(entry.NeedId)
                    ? string.Empty
                    : entry.NeedId.Trim();

                entry.MinIntensity = Mathf.Clamp01(entry.MinIntensity);
                entry.MaxIntensity = Mathf.Clamp(entry.MaxIntensity, entry.MinIntensity, 1f);

                _needProfiles[index] = entry;
            }
        }
    }
}