using UnityEngine;
using UnityEngine.AI;

public class BTMoveToPosition : BTBaseNode
{
    private NavMeshAgent _agent;
    private string _BBTargetPosition;
    private Vector3 _targetPosition;

    private float _maxStuckTimer = 3f;
    private float _stuckTimer = 3f;

    private bool _interruptable = false;

    public BTMoveToPosition(NavMeshAgent agent, string targetPosition, bool interruptable = false)
    {
        _agent = agent;
        _BBTargetPosition = targetPosition;
        _interruptable = interruptable;
    }

    public BTMoveToPosition(NavMeshAgent agent, Vector3 targetPosition)
    {
        _agent = agent;
        _targetPosition = targetPosition;
    }

    protected override void OnEnter()
    {
        if (_BBTargetPosition == null || _BBTargetPosition == "") return;
        _targetPosition = blackboard.GetVariable<Vector3>(_BBTargetPosition);
    }

    protected override TaskStatus OnUpdate()
    {
        if (_agent == null) { return TaskStatus.Failed; }
        if (_agent.pathEndPosition != _targetPosition)
        {
            _agent.SetDestination(_targetPosition);
        }
        if (_agent.pathPending) { return TaskStatus.Running; }
        if (_agent.hasPath && _agent.path.status == NavMeshPathStatus.PathPartial) { return TaskStatus.Failed; }

        if (Vector3.Distance(_agent.transform.position, _targetPosition) <= 0.3f)
        {
            return TaskStatus.Success;
        }

        //checks if agent is stuck for too long and returns fail if true
        if (_agent.velocity.magnitude < 0.05f)
        {
            if (_stuckTimer > 0f)
            {
                _stuckTimer -= Time.deltaTime;
            }
            if (_stuckTimer <= 0f)
            {
                return TaskStatus.Failed;
            }
        }
        else
        {
            _stuckTimer = _maxStuckTimer;
        }
        return _interruptable ? TaskStatus.Success : TaskStatus.Running;
    }
}
