using Game.World.EntityRuntime;

namespace Game.World.UtilityAi
{
    public interface IUtilityOption
    {
        EntityRoot TargetRoot { get; }

        void WriteFacts(IUtilityFactWriter writer);

        bool IsEquivalentTo(IUtilityOption other);
    }
}