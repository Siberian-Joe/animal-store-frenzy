namespace Game.World.UtilityAi
{
    public sealed class DistanceToTargetConsiderationPart : UtilityConsiderationPart
    {
        private IUtilityFactAccessor<DistanceToTargetFact> _fact;

        protected override void OnBind(UtilityFactRuntime facts) => _fact = facts.GetAccessor<DistanceToTargetFact>();

        protected override float Measure() =>
            _fact is { HasValue: true }
                ? 1f - _fact.Value.NormalizedDistance
                : 0f;
    }
}