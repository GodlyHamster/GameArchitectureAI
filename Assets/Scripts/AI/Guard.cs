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

    [SerializeField]
    private GameObject _weaponObject;
    private bool _hasWeapon = false;

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
                    new BTMoveToPosition(_agent, "playerLastSeenPos", true),
                    new BTDebug("pew pew")
                ),
                new BTSequence(
                    new BTCondition("seesPlayer"),
                    new BTInverter(new BTCondition("hasWeapon")),
                    new BTMoveToPosition(_agent, "weaponLocation"),
                    new BTGrabWeapon(_agent, "hasWeapon"),
                    new BTMoveToPosition(_agent, "playerLastSeenPos", true)
                )
            ),
            new BTSequence(
                new BTPatrol(_agent, linkedPatrolPoints, "currentPatrolPoint", true),
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

        //check if guard can see player
        Vector3 rayDir = player.transform.position - transform.position;
        Physics.Raycast(transform.position, rayDir, out RaycastHit hit, 5f);

        bool canSeePlayer = false;
        if (hit.transform != null)
        {
            canSeePlayer = hit.transform.CompareTag("Player");
        }
        _blackboard.SetVariable("seesPlayer", (canSeePlayer && !_isSmoked));
        if (canSeePlayer && !_isSmoked)
        {
            _blackboard.SetVariable("playerLastSeenPos", player.transform.position);
        }

        _hasWeapon = _blackboard.GetVariable<bool>("hasWeapon");
        if (_hasWeapon && !_weaponObject.activeSelf)
        {
            _weaponObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("smokeBomb"))
        {
            _isSmoked = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("smokeBomb"))
        {
            _isSmoked = false;
        }
    }
}
