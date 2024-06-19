using System.Collections;

public interface IUtilityAction
{
    float Evaluate(AIContext context, IActionScoreEvaluator scoreEvaluator);
    IEnumerator ExecuteCoroutine(UtilityBehaviour utilityBehaviour);
}