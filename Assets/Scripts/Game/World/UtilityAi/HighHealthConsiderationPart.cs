namespace Game.World.UtilityAi
{
    public sealed class HighHealthConsiderationPart : UtilityConsiderationPart
    {
        private IUtilityFactAccessor<SelfHealthFact> _fact;

        protected override void OnBind(UtilityFactRuntime facts) => _fact = facts.GetAccessor<SelfHealthFact>();

        protected override float Measure() =>
            _fact is { HasValue: true }
                ? _fact.Value.NormalizedValue
                : 0f;
    }
}