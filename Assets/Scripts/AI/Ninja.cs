using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Ninja : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [Header("Debugging")]
    [SerializeField]
    private TextMeshProUGUI stateText;
    [SerializeField]
    private RectTransform canvas;

    private BTBaseNode _behaviorTree;
    private NavMeshAgent _agent;

    private Blackboard _blackboard = new Blackboard();

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _blackboard.SetVariable("playerPosition", player.transform.position);
        _blackboard.SetVariable("playerOutRange", false);

        _behaviorTree = new BTSelector(
            new BTSequence(
                new BTCondition("playerOutRange"),
                new BTMoveToPosition(_agent, "playerPosition")
                )
            );
        _behaviorTree.SetupBlackboard(_blackboard);
    }

    private void FixedUpdate()
    {
        _blackboard.SetVariable("playerPosition", player.transform.position);
        _blackboard.SetVariable("playerOutRange", Vector3.Distance(transform.position, player.transform.position) >= 3f);

        TaskStatus result = _behaviorTree.Tick();

        BTBaseNode currentState = _behaviorTree.GetState();
        stateText.text = currentState.ToString();
        canvas.rotation = Camera.main.transform.rotation;
    }
}
