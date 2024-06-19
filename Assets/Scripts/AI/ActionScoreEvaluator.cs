using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ActionScoreEvaluator : IActionScoreEvaluator
{
    public float EvaluateScore(AIContext context, List<IUtilityConsideration> considerations, StringBuilder sb)
    {
        var score = 1f;

        foreach (var consideration in considerations)
        {
            var considerationScore = consideration.Score(context);

            score *= considerationScore;

            sb.Append($"{consideration.GetType().Name}: {considerationScore:F2}, ");

            if (score == 0)
                return 0;
        }

        var originalScore = score;
        var modFactor = 1 - 1 / (float)considerations.Count;
        var makeupValue = (1 - originalScore) * modFactor;

        Debug.Log($"{sb}Final: {originalScore + makeupValue * originalScore:F2}");

        return originalScore + makeupValue * originalScore;
    }
}