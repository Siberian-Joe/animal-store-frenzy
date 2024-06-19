using System.Collections.Generic;

public interface IHighestScoreSelector
{
    IUtilityAction SelectBestAction(List<IUtilityAction> actions, AIContext context,
        IActionScoreEvaluator scoreEvaluator);
}