using JetBrains.Annotations;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Ninja : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject guard;
    [SerializeField]
    private SmokeBomb smokebomb;
    private float _smokeBombCooldown = 0f;

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
        _blackboard.SetVariable("seesGuard", false);
        _blackboard.SetVariable("guardLocation", Vector3.zero);
        _blackboard.SetVariable("smokeBombCooldown", 0f);

        _behaviorTree = new BTSelector(
            new BTSequence(
                new BTCondition(() => { return _smokeBombCooldown <= 0f; }),
                new BTCondition(() => { return Vector3.Distance(transform.position, guard.transform.position) <= 5f; }),
                new BTThrowSmokeBomb("guardLocation", smokebomb)
                ),
            new BTSequence(
                new BTCondition(() => { return Vector3.Distance(transform.position, player.transform.position) >= 3f; }),
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
        _blackboard.SetVariable("guardLocation", guard.transform.position);
        _smokeBombCooldown = _blackboard.GetVariable<float>("smokeBombCooldown");
        if (_smokeBombCooldown > 0)
        {
            _blackboard.SetVariable("smokeBombCooldown", _smokeBombCooldown - Time.deltaTime);
        }
    }
}
