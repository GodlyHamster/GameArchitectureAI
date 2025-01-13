using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ninja : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private BTBaseNode behaviorTree;
    private NavMeshAgent agent;

    private Blackboard _blackboard = new Blackboard();

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _blackboard.SetVariable("playerPosition", player.transform.position);
        _blackboard.SetVariable("randomvar", new Vector3(0, 0, 0));

        behaviorTree = new BTSequence(
            new BTMoveToPosition(agent, "playerPosition")
            );
        behaviorTree.SetupBlackboard(_blackboard);
    }

    private void FixedUpdate()
    {
        TaskStatus result = behaviorTree.Tick();
        _blackboard.SetVariable("playerPosition", player.transform.position);
    }
}
