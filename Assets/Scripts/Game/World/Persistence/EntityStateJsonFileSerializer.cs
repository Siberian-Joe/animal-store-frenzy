using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using JsonUtility = UnityEngine.JsonUtility;

namespace Game.World.Persistence
{
    internal sealed class EntityStateJsonFileSerializer
    {
        public string Serialize(IEnumerable<EntityState> states)
        {
            if (states == null)
                throw new ArgumentNullException(nameof(states));

            var entityDtos = new List<EntityStateDto>();

            foreach (var state in states)
            {
                if (state == null)
                    continue;

                var entityDto = new EntityStateDto
                {
                    Id = state.Id.Value,
                    BlueprintId = state.BlueprintId,
                    Slices = new List<StateSliceDto>()
                };

                foreach (var slice in state.EnumerateSlices())
                {
                    entityDto.Slices.Add(new StateSliceDto
                    {
                        OwnerScopeId = slice.Key.OwnerScopeId,
                        StateTypeId = slice.Key.StateTypeId,
                        Json = JsonUtility.ToJson(slice.State, false)
                    });
                }

                entityDto.Slices.Sort(CompareSlices);
                entityDtos.Add(entityDto);
            }

            entityDtos.Sort((left, right) => string.CompareOrdinal(left.Id, right.Id));

            return JsonUtility.ToJson(new SnapshotDto
            {
                Entities = entityDtos
            }, true);
        }

        public IReadOnlyList<EntityState> Deserialize(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return Array.Empty<EntityState>();

            var snapshot = JsonUtility.FromJson<SnapshotDto>(json);
            return snapshot == null
                ? throw new InvalidOperationException("State snapshot JSON could not be deserialized.")
                : DeserializeStates(snapshot.Entities);
        }

        private static IReadOnlyList<EntityState> DeserializeStates(List<EntityStateDto> entities)
        {
            if (entities == null || entities.Count == 0)
                return Array.Empty<EntityState>();

            var states = new List<EntityState>(entities.Count);

            foreach (var entityDto in entities)
            {
                if (string.IsNullOrWhiteSpace(entityDto.Id))
                    throw new InvalidOperationException("State snapshot contains an entity with an empty id.");

                var entityState = new EntityState(new EntityId(entityDto.Id), entityDto.BlueprintId);

                if (entityDto.Slices != null)
                {
                    foreach (var sliceDto in entityDto.Slices)
                    {
                        var stateType = ResolveStateType(sliceDto.StateTypeId);
                        var key = string.IsNullOrWhiteSpace(sliceDto.OwnerScopeId)
                            ? StateSlotKey.For(stateType)
                            : StateSlotKey.For(stateType, sliceDto.OwnerScopeId);
                        entityState.Add(key, DeserializeStateSlice(sliceDto, stateType));
                    }
                }

                states.Add(entityState);
            }

            return states;
        }

        private static Type ResolveStateType(string stateTypeId)
        {
            if (string.IsNullOrWhiteSpace(stateTypeId))
                throw new InvalidOperationException("State slice does not specify a CLR state type.");

            var stateType = Type.GetType(stateTypeId, false);
            if (stateType == null)
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    stateType = assembly.GetType(stateTypeId, false);
                    if (stateType != null)
                        break;
                }
            }

            if (stateType == null || typeof(IEntityStateData).IsAssignableFrom(stateType) == false)
            {
                throw new InvalidOperationException(
                    $"State slice references unresolved state type '{stateTypeId}'. " +
                    "The current persistence contract prioritizes simplicity and resolves payloads by CLR type name.");
            }

            return stateType;
        }

        private static IEntityStateData DeserializeStateSlice(StateSliceDto sliceDto, Type stateType)
        {
            if (string.IsNullOrWhiteSpace(sliceDto.Json))
            {
                throw new InvalidOperationException(
                    $"State slice '{sliceDto.StateTypeId}' has no JSON payload.");
            }

            if (JsonUtility.FromJson(sliceDto.Json, stateType) is not IEntityStateData state)
            {
                throw new InvalidOperationException(
                    $"State slice '{sliceDto.StateTypeId}' could not be deserialized as '{stateType.Name}'.");
            }

            return state;
        }

        private static int CompareSlices(StateSliceDto left, StateSliceDto right)
        {
            var ownerComparison = string.CompareOrdinal(
                left.OwnerScopeId ?? string.Empty,
                right.OwnerScopeId ?? string.Empty);
            if (ownerComparison != 0)
                return ownerComparison;

            return string.CompareOrdinal(left.StateTypeId, right.StateTypeId);
        }

        [Serializable]
        private sealed class SnapshotDto
        {
            public List<EntityStateDto> Entities = new();
        }

        [Serializable]
        private sealed class EntityStateDto
        {
            public string Id;
            public string BlueprintId;
            public List<StateSliceDto> Slices = new();
        }

        [Serializable]
        private sealed class StateSliceDto
        {
            public string OwnerScopeId;
            public string StateTypeId;
            public string Json;
        }
    }
}