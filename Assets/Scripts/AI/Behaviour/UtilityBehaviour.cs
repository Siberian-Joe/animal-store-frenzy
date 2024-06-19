using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UtilityBehaviour : MonoBehaviour
{
    [SerializeReference, SelectImplementation]
    private List<IUtilityAction> _utilityActions;

    [field: SerializeField] public StatCollection StatCollection { get; private set; } = new();

    public NavMeshAgent NavMeshAgent { get; private set; }

    private readonly UtilityAI _utilityAI = new();

    private AIContext _aiContext;

    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        _aiContext = new AIContext(StatCollection);

        foreach (var utilityAction in _utilityActions)
            _utilityAI.AddAction(utilityAction);
    }

    private void Update()
    {
        _utilityAI.ExecuteBestAction(this, _aiContext);
    }
}