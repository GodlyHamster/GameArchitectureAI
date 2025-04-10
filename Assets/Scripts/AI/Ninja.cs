using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
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

    private List<Vector3> _hidingSpots = new List<Vector3>();

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
        foreach (var spot in GameObject.FindGameObjectsWithTag("HidingSpot"))
        {
            _hidingSpots.Add(spot.transform.position);
        }

        _blackboard.SetVariable("playerPosition", player.transform.position);
        _blackboard.SetVariable("seesGuard", false);
        _blackboard.SetVariable("guardLocation", Vector3.zero);
        _blackboard.SetVariable("smokeBombCooldown", 0f);
        _blackboard.SetVariable("closestHidingSpot", Vector3.zero);

        _behaviorTree = new BTSelector(
            new BTSequence(
                new BTCondition(() => { return _smokeBombCooldown <= 0f; }),
                new BTCondition(() => { return Vector3.Distance(transform.position, guard.transform.position) <= 5f; }),
                new BTMoveToPosition(_agent, "closestHidingSpot"),
                new BTThrowSmokeBomb("guardLocation", smokebomb)
                ),
            new BTSequence(
                new BTCondition(() => { return Vector3.Distance(transform.position, player.transform.position) >= 3f; }),
                new BTMoveToPosition(_agent, "playerPosition", true)
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

        _blackboard.SetVariable("closestHidingSpot", GetClosestHidingSpot());
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

    private Vector3 GetClosestHidingSpot()
    {
        if (_hidingSpots == null || _hidingSpots.Count == 0) return Vector3.zero;
        _hidingSpots = _hidingSpots.OrderBy((v) => (transform.position - v ).sqrMagnitude).ToList();
        return _hidingSpots[0];
    }
}
