namespace Game.World.UtilityAi
{
    public sealed class NeedIntensityConsiderationPart : UtilityConsiderationPart
    {
        private IUtilityFactAccessor<NeedIntensityFact> _fact;

        protected override void OnBind(UtilityFactRuntime facts) => _fact = facts.GetAccessor<NeedIntensityFact>();

        protected override float Measure() =>
            _fact is { HasValue: true }
                ? _fact.Value.NormalizedValue
                : 0f;
    }
}