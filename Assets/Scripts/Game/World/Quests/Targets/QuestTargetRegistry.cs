using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;

namespace Game.World.Quests.Targets
{
    public sealed class QuestTargetRegistry : IQuestTargetRegistry, IQuestTargetResolver
    {
        private readonly Dictionary<QuestTargetId, EntityRoot> _rootsById = new();

        public void Register(QuestTargetId id, EntityRoot root)
        {
            if (root == false)
                throw new ArgumentNullException(nameof(root));

            if (_rootsById.TryGetValue(id, out var existing) && existing != root)
            {
                throw new InvalidOperationException(
                    $"Duplicate quest target '{id}' detected: '{existing.name}' and '{root.name}'.");
            }

            _rootsById[id] = root;
        }

        public void Unregister(QuestTargetId id, EntityRoot root)
        {
            if (root == false)
                return;

            if (_rootsById.TryGetValue(id, out var existing) && existing == root)
                _rootsById.Remove(id);
        }

        public bool TryResolve(QuestTargetId id, out EntityRoot root) => _rootsById.TryGetValue(id, out root) && root;
    }
}