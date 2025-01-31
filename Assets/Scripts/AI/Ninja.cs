using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Ninja : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject guard;
    [SerializeField]
    private GameObject smokebomb;

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
        _blackboard.SetVariable("seesGuard", false);
        _blackboard.SetVariable("guardLocation", Vector3.zero);
        _blackboard.SetVariable("smokeBombActive", false);

        _behaviorTree = new BTSelector(
            new BTSequence(
                new BTInverter(new BTCondition("smokeBombActive")),
                new BTCondition("seesGuard"),
                new BTThrowSmokeBomb("guardLocation", smokebomb)
                ),
            new BTSequence(
                new BTCondition("playerOutRange"),
                new BTMoveToPosition(_agent, "playerPosition")
                )
            );
        _behaviorTree.SetupBlackboard(_blackboard);
    }

    private void FixedUpdate()
    {
        TaskStatus result = _behaviorTree.Tick();

        BTBaseNode currentState = _behaviorTree.GetState();
        stateText.text = currentState.ToString();
        canvas.rotation = Camera.main.transform.rotation;
    }

    private void Update()
    {
        _blackboard.SetVariable("playerPosition", player.transform.position);
        _blackboard.SetVariable("playerOutRange", Vector3.Distance(transform.position, player.transform.position) >= 3f);
        _blackboard.SetVariable("seesGuard", Vector3.Distance(transform.position, guard.transform.position) <= 5f);
        _blackboard.SetVariable("guardLocation", guard.transform.position);
    }
}
