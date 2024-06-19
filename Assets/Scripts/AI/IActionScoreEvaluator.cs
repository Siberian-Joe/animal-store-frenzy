using System.Collections.Generic;
using System.Text;

public interface IActionScoreEvaluator
{
    float EvaluateScore(AIContext context, List<IUtilityConsideration> considerations, StringBuilder sb);
}