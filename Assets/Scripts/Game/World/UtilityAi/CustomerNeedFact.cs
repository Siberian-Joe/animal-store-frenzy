namespace Game.World.UtilityAi
{
    public readonly struct CustomerNeedFact
    {
        public CustomerNeedFact(CustomerNeedHandle need) => Need = need;

        public CustomerNeedHandle Need { get; }
    }
}