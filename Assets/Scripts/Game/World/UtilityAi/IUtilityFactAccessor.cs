namespace Game.World.UtilityAi
{
    public interface IUtilityFactAccessor<out TFact>
        where TFact : struct
    {
        bool HasValue { get; }

        TFact Value { get; }
    }
}