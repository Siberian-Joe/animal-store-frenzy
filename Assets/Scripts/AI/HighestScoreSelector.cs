using System.Collections.Generic;

public class HighestScoreSelector : IHighestScoreSelector
{
    public IUtilityAction SelectBestAction(List<IUtilityAction> actions, AIContext context,
        IActionScoreEvaluator scoreEvaluator)
    {
        var bestAction = default(IUtilityAction);
        var highestScore = 0f;

        foreach (var action in actions)
        {
            var score = action.Evaluate(context, scoreEvaluator);

            if (score > highestScore)
            {
                highestScore = score;
                bestAction = action;
            }
        }

        return bestAction;
    }
}