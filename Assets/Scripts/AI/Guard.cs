using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
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
        Blackboard blackboard = new Blackboard();
        blackboard.SetVariable("patrolPos1", new Vector3(-14, 0, 0));
        blackboard.SetVariable("patrolPos2", new Vector3(-14, 0, 9));

        behaviorTree = new BTSequence(
            new BTMoveToPosition(agent, "patrolPos1"),
            new BTWait(2f),
            new BTMoveToPosition(agent, "patrolPos2"),
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
