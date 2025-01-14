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

        behaviorTree = new BTSelector(
            new BTMoveToPosition(agent, "playerPosition")
            );
        behaviorTree.SetupBlackboard(_blackboard);
    }

    private void FixedUpdate()
    {
        _blackboard.SetVariable("playerPosition", player.transform.position);

        TaskStatus result = behaviorTree.Tick();

        BTBaseNode currentState = behaviorTree.GetState();
        stateText.text = currentState.ToString();
        canvas.rotation = Camera.main.transform.rotation;
    }
}
