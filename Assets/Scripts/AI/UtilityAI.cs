using System.Collections;
using System.Collections.Generic;

public class UtilityAI
{
    private readonly List<IUtilityAction> _actions = new();
    private readonly IActionScoreEvaluator _scoreEvaluator;
    private readonly IHighestScoreSelector _scoreSelector;
    private bool _isActionRunning;

    public UtilityAI(IActionScoreEvaluator scoreEvaluator = null, IHighestScoreSelector scoreSelector = null)
    {
        _scoreEvaluator = scoreEvaluator ?? new ActionScoreEvaluator();
        _scoreSelector = scoreSelector ?? new HighestScoreSelector();
    }

    public void AddAction(IUtilityAction action)
    {
        _actions.Add(action);
    }

    public void ExecuteBestAction(UtilityBehaviour utilityBehaviour, AIContext context)
    {
        if (_isActionRunning)
            return;

        var bestAction = _scoreSelector.SelectBestAction(_actions, context, _scoreEvaluator);

        if (bestAction != null)
            utilityBehaviour.StartCoroutine(ExecuteActionCoroutine(bestAction, utilityBehaviour));
    }

    private IEnumerator ExecuteActionCoroutine(IUtilityAction action, UtilityBehaviour utilityBehaviour)
    {
        _isActionRunning = true;
        yield return action.ExecuteCoroutine(utilityBehaviour);
        _isActionRunning = false;
    }
}