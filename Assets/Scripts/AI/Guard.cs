using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
     
    [SerializeField]
    private List<Vector3> patrolPoints = new List<Vector3>();
    private LinkedList<Vector3> linkedPatrolPoints = new LinkedList<Vector3>();

    [SerializeField]
    private TextMeshProUGUI stateText;
    [SerializeField]
    private RectTransform canvas;

    private BTBaseNode behaviorTree;
    private NavMeshAgent agent;

    Blackboard _blackboard = new Blackboard();

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

        _blackboard.SetVariable("currentPatrolPoint", linkedPatrolPoints.First);
        _blackboard.SetVariable("playerPosition", player.transform.position);
        _blackboard.SetVariable("playerInRange", false);

        behaviorTree = new BTSelector(
            new BTSequence(
                new BTCondition("playerInRange"),
                new BTMoveToPosition(agent, "playerPosition")
                ),
            new BTSequence(
                new BTPatrol(agent, linkedPatrolPoints, "currentPatrolPoint"),
                new BTWait(2f)
                )
            );
        behaviorTree.SetupBlackboard( _blackboard );
    }

    private void FixedUpdate()
    {
        _blackboard.SetVariable("playerPosition", player.transform.position);
        _blackboard.SetVariable("playerInRange", Vector3.Distance(transform.position, player.transform.position) <= 5f);

        TaskStatus result = behaviorTree.Tick();

        BTBaseNode currentState = behaviorTree.GetState();
        stateText.text = currentState.ToString();
        canvas.rotation = Camera.main.transform.rotation;
    }
}
