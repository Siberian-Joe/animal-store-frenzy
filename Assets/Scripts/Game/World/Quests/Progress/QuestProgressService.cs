using System;
using System.Collections.Generic;
using Game.World.Quests.Authoring;
using Game.World.Quests.Targets;
using R3;

namespace Game.World.Quests.Progress
{
    public sealed class QuestProgressService : IQuestProgressService, IDisposable
    {
        private readonly IReadOnlyList<QuestDefinition> _definitions;
        private readonly IQuestTargetResolver _targetResolver;
        private readonly IQuestProgressStateStore _stateStore;
        private readonly Subject<QuestProgressSnapshot> _progressChanged = new();
        private readonly Subject<QuestSnapshot> _questCompleted = new();

        private QuestProgressState _state;
        private bool _isInitialized;

        public QuestProgressService(
            List<QuestDefinition> definitions,
            IQuestTargetResolver targetResolver,
            IQuestProgressStateStore stateStore)
        {
            _definitions = definitions ?? throw new ArgumentNullException(nameof(definitions));
            _targetResolver = targetResolver ?? throw new ArgumentNullException(nameof(targetResolver));
            _stateStore = stateStore ?? throw new ArgumentNullException(nameof(stateStore));
        }

        public QuestProgressSnapshot Current => BuildSnapshot();
        public Observable<QuestProgressSnapshot> ProgressChanged => _progressChanged;
        public Observable<QuestSnapshot> QuestCompleted => _questCompleted;

        public void Initialize()
        {
            if (_isInitialized)
                return;

            foreach (var definition in _definitions)
            {
                if (definition == false)
                    throw new InvalidOperationException(
                        $"{nameof(QuestProgressService)} received a null quest definition.");

                definition.Validate();
            }

            _state = _stateStore.Load() ?? new QuestProgressState();
            EnsureStateShape();
            CheckProgress(emitCompletion: false);
            _isInitialized = true;
            _progressChanged.OnNext(Current);
        }

        public void CheckProgress(bool emitCompletion = true)
        {
            EnsureInitializedForCheck();

            var changed = false;
            var completedSnapshots = new List<QuestSnapshot>();

            foreach (var definition in _definitions)
            {
                var questState = FindOrCreateQuestState(definition);

                foreach (var objectiveDefinition in definition.Objectives)
                {
                    var objectiveState = FindOrCreateObjectiveState(questState, objectiveDefinition);
                    if (objectiveState.IsCompleted)
                        continue;

                    if (objectiveDefinition.Condition.IsMet(_targetResolver) == false)
                        continue;

                    objectiveState.IsCompleted = true;
                    changed = true;
                }

                if (questState.IsCompleted || AreAllCurrentObjectivesCompleted(definition, questState) == false)
                    continue;

                questState.IsCompleted = true;
                changed = true;

                if (emitCompletion)
                    completedSnapshots.Add(BuildQuestSnapshot(definition, questState));
            }

            if (changed == false)
                return;

            _stateStore.Save(_state);
            var snapshot = Current;
            _progressChanged.OnNext(snapshot);

            foreach (var completed in completedSnapshots)
                _questCompleted.OnNext(completed);
        }

        public void Dispose()
        {
            _progressChanged.Dispose();
            _questCompleted.Dispose();
        }

        private void EnsureStateShape()
        {
            _state.Quests ??= new List<QuestState>();
            foreach (var definition in _definitions)
            {
                var questState = FindOrCreateQuestState(definition);
                questState.Objectives ??= new List<QuestObjectiveState>();

                foreach (var objective in definition.Objectives)
                    _ = FindOrCreateObjectiveState(questState, objective);
            }

            _stateStore.Save(_state);
        }

        private QuestProgressSnapshot BuildSnapshot()
        {
            EnsureInitializedForRead();

            var quests = new List<QuestSnapshot>(_definitions.Count);
            foreach (var definition in _definitions)
            {
                var questState = FindOrCreateQuestState(definition);
                quests.Add(BuildQuestSnapshot(definition, questState));
            }

            return new QuestProgressSnapshot(quests);
        }

        private QuestSnapshot BuildQuestSnapshot(QuestDefinition definition, QuestState state)
        {
            var objectives = new List<QuestObjectiveSnapshot>(definition.Objectives.Count);
            foreach (var objectiveDefinition in definition.Objectives)
            {
                var objectiveState = FindOrCreateObjectiveState(state, objectiveDefinition);
                objectives.Add(new QuestObjectiveSnapshot(
                    objectiveDefinition.Id.Value,
                    objectiveDefinition.ObjectiveText,
                    objectiveState.IsCompleted));
            }

            return new QuestSnapshot(
                definition.Id.Value,
                definition.Title,
                definition.Summary,
                state.IsCompleted,
                objectives);
        }

        private QuestState FindOrCreateQuestState(QuestDefinition definition)
        {
            foreach (var quest in _state.Quests)
            {
                if (quest == null)
                    continue;

                if (string.Equals(quest.QuestId, definition.Id.Value, StringComparison.Ordinal) == false)
                    continue;

                quest.Objectives ??= new List<QuestObjectiveState>();
                return quest;
            }

            var created = new QuestState
            {
                QuestId = definition.Id.Value,
                Objectives = new List<QuestObjectiveState>()
            };

            _state.Quests.Add(created);
            return created;
        }

        private static QuestObjectiveState FindOrCreateObjectiveState(
            QuestState questState,
            QuestObjectiveDefinition definition)
        {
            questState.Objectives ??= new List<QuestObjectiveState>();

            foreach (var objective in questState.Objectives)
            {
                if (objective == null)
                    continue;

                if (string.Equals(objective.ObjectiveId, definition.Id.Value, StringComparison.Ordinal))
                    return objective;
            }

            var created = new QuestObjectiveState
            {
                ObjectiveId = definition.Id.Value
            };

            questState.Objectives.Add(created);
            return created;
        }

        private static bool AreAllCurrentObjectivesCompleted(QuestDefinition definition, QuestState questState)
        {
            foreach (var objectiveDefinition in definition.Objectives)
            {
                var objectiveState = FindOrCreateObjectiveState(questState, objectiveDefinition);
                if (objectiveState.IsCompleted == false)
                    return false;
            }

            return true;
        }

        private void EnsureInitializedForRead() => _state ??= new QuestProgressState();

        private void EnsureInitializedForCheck()
        {
            if (_state == null)
                Initialize();
        }
    }
}