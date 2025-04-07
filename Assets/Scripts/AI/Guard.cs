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

    private BTBaseNode _behaviorTree;
    private NavMeshAgent _agent;

    private Blackboard _blackboard = new Blackboard();

    private bool _isSmoked = false;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        foreach (Vector3 point in patrolPoints)
        {
            linkedPatrolPoints.AddLast(point);
        }

        _blackboard.SetVariable("currentPatrolPoint", linkedPatrolPoints.First);
        _blackboard.SetVariable("playerLastSeenPos", player.transform.position);
        _blackboard.SetVariable("seesPlayer", false);
        _blackboard.SetVariable("weaponLocation", GameObject.FindGameObjectWithTag("Weapon").transform.position);
        _blackboard.SetVariable("hasWeapon", false);

        _behaviorTree = new BTSelector(
            new BTSelector(
                new BTSequence(
                    new BTCondition("seesPlayer"),
                    new BTCondition("hasWeapon"),
                    new BTMoveToPosition(_agent, "playerLastSeenPos"),
                    new BTDebug("pew pew")
                    ),
                new BTSequence(
                    new BTCondition("seesPlayer"),
                    new BTInverter(new BTCondition("hasWeapon")),
                    new BTMoveToPosition(_agent, "weaponLocation"),
                    new BTGrabWeapon(_agent, "hasWeapon"),
                    new BTMoveToPosition(_agent, "playerLastSeenPos")
                    )
                ),
            new BTSequence(
                new BTPatrol(_agent, linkedPatrolPoints, "currentPatrolPoint"),
                new BTWait(2f)
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
        Vector3 rayDir = player.transform.position - transform.position;
        Physics.Raycast(transform.position, rayDir, out RaycastHit hit, 5f);

        bool canSeePlayer = false;
        if (hit.transform != null)
        {
            canSeePlayer = !hit.transform.gameObject.CompareTag("smokeBomb");
        }
        _blackboard.SetVariable("seesPlayer", (canSeePlayer && !_isSmoked));
        if (!_isSmoked)
        {
            _blackboard.SetVariable("playerLastSeenPos", player.transform.position);
        }
    }
}
