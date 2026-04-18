using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public interface IEntityBlueprintCatalog
    {
        bool TryGetPrefab(string blueprintId, out EntityRoot prefab);
    }
}
