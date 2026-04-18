using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public sealed class FileEntityStateStore : IEntityStateStore, IDisposable
    {
        private static readonly UTF8Encoding Utf8WithoutBom = new(false);

        private readonly Dictionary<EntityId, EntityState> _states = new();
        private readonly EntityStateJsonFileSerializer _serializer = new();
        private readonly string _filePath;

        private string _lastPersistedSnapshot = string.Empty;

        public FileEntityStateStore(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or whitespace.", nameof(filePath));

            _filePath = filePath;
            HasSnapshot = File.Exists(_filePath);

            LoadFromFile();
        }

        public bool HasSnapshot { get; }

        public IReadOnlyCollection<EntityState> All => _states.Values;

        public bool TryGet(EntityId id, out EntityState state) =>
            _states.TryGetValue(id, out state);

        public void Save(EntityState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            _states[state.Id] = state;
            PersistIfChanged();
        }

        public bool Remove(EntityId id)
        {
            if (_states.Remove(id) == false)
                return false;

            PersistIfChanged();
            return true;
        }

        public void Dispose()
        {
            PersistIfChanged();
        }

        private void LoadFromFile()
        {
            if (HasSnapshot == false)
                return;

            var json = File.ReadAllText(_filePath, Utf8WithoutBom);

            if (string.IsNullOrWhiteSpace(json))
            {
                _lastPersistedSnapshot = _serializer.Serialize(Array.Empty<EntityState>());
                return;
            }

            var states = _serializer.Deserialize(json);
            foreach (var state in states)
            {
                if (state == null)
                    continue;

                _states[state.Id] = state;
            }

            _lastPersistedSnapshot = json;
        }

        private void PersistIfChanged()
        {
            var snapshot = _serializer.Serialize(_states.Values);
            if (string.Equals(snapshot, _lastPersistedSnapshot, StringComparison.Ordinal))
                return;

            var directoryPath = Path.GetDirectoryName(_filePath);
            if (string.IsNullOrWhiteSpace(directoryPath) == false)
                Directory.CreateDirectory(directoryPath);

            File.WriteAllText(_filePath, snapshot, Utf8WithoutBom);
            _lastPersistedSnapshot = snapshot;
        }
    }
}