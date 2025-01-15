using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [Header("Debugging")]
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
        _blackboard.SetVariable("playerLastSeenPos", player.transform.position);
        _blackboard.SetVariable("playerInRange", false);
        _blackboard.SetVariable("weaponLocation", Vector3.zero);
        _blackboard.SetVariable("hasWeapon", false);

        behaviorTree = new BTSelector(
            new BTSelector(
                new BTSequence(
                    new BTCondition("playerInRange"),
                    new BTCondition("hasWeapon"),
                    new BTMoveToPosition(agent, "playerLastSeenPos")
                    ),
                new BTSequence(
                    new BTCondition("playerInRange"),
                    new BTInverter(new BTCondition("hasWeapon")),
                    new BTMoveToPosition(agent, "weaponLocation"),
                    new BTMoveToPosition(agent, "playerLastSeenPos")
                    )
                ),
            new BTSequence(
                new BTPatrol(agent, linkedPatrolPoints, "currentPatrolPoint"),
                new BTWait(2f)
                )
            );
        behaviorTree.SetupBlackboard(_blackboard);
    }

    private void FixedUpdate()
    {
        bool playerInRange = Vector3.Distance(transform.position, player.transform.position) <= 5f;
        _blackboard.SetVariable("playerInRange", playerInRange);
        if (playerInRange)
        {
            _blackboard.SetVariable("playerLastSeenPos", player.transform.position);
        }

        TaskStatus result = behaviorTree.Tick();

        BTBaseNode currentState = behaviorTree.GetState();
        stateText.text = currentState.ToString();
        canvas.rotation = Camera.main.transform.rotation;
    }
}
