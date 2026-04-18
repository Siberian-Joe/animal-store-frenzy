namespace Game.World.UtilityAi
{
    public sealed class LowHealthConsiderationPart : UtilityConsiderationPart
    {
        private IUtilityFactAccessor<SelfHealthFact> _fact;

        protected override void OnBind(UtilityFactRuntime facts)
        {
            _fact = facts.GetAccessor<SelfHealthFact>();
        }

        protected override float Measure()
        {
            return _fact is { HasValue: true }
                ? 1f - _fact.Value.NormalizedValue
                : 0f;
        }
    }
}