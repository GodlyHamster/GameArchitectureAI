using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> patrolPoints = new List<Vector3>();
    private LinkedList<Vector3> linkedPatrolPoints = new LinkedList<Vector3>();

    [SerializeField]
    private TextMeshProUGUI stateText;

    private BTBaseNode behaviorTree;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        foreach (Vector3 point in patrolPoints)
        {
            linkedPatrolPoints.AddLast(point);
        }

        Blackboard blackboard = new Blackboard();
        blackboard.SetVariable("currentPatrolPoint", linkedPatrolPoints.First);

        behaviorTree = new BTSequence(
            new BTPatrol(agent, linkedPatrolPoints, "currentPatrolPoint"),
            new BTWait(2f)
            );
        behaviorTree.SetupBlackboard( blackboard );
    }

    private void FixedUpdate()
    {
        TaskStatus result = behaviorTree.Tick();
        stateText.text = ((BTSequence)behaviorTree).GetCurrentState().ToString() + ": " + result;
    }
}
