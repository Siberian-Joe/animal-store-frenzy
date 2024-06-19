using System;
using UnityEngine;

[Serializable]
public class DesireToCheckoutConsideration : IUtilityConsideration
{
    [SerializeField] private AnimationCurve _desireToCheckoutCurve;

    public float Score(AIContext context)
    {
        var desireToCheckout = context.StatCollection.GetStat<DesireToCheckout>();
        return _desireToCheckoutCurve.Evaluate(desireToCheckout.Value / desireToCheckout.MaxValue);
    }
}