namespace Game.World.UtilityAi
{
    public interface IUtilityFactWriter
    {
        void Set<TFact>(in TFact fact)
            where TFact : struct;
    }
}