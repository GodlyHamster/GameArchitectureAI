using UnityEngine;
using UnityEngine.AI;

public class BTMoveToPosition : BTBaseNode
{
    private NavMeshAgent _agent;
    private string _BBTargetPosition;
    private Vector3 _targetPosition;

    private float maxStuckTimer = 3f;
    private float stuckTimer = 3f;

    public BTMoveToPosition(NavMeshAgent agent, string targetPosition)
    {
        _agent = agent;
        _BBTargetPosition = targetPosition;
    }

    protected override void OnEnter()
    {
        _targetPosition = blackboard.GetVariable<Vector3>(_BBTargetPosition);
    }

    protected override TaskStatus OnUpdate()
    {
        if (_agent == null) { return TaskStatus.Failed; }
        if (_agent.pathPending) { return TaskStatus.Running; }
        if (_agent.hasPath && _agent.path.status == NavMeshPathStatus.PathPartial) { return TaskStatus.Failed; }
        if (_agent.pathEndPosition != _targetPosition)
        {
            _agent.SetDestination(_targetPosition);
        }

        //checks if agent is stuck for too long and returns fail if true
        if (Vector3.Distance(_agent.transform.position, _targetPosition) <= 0.3f)
        {
            return TaskStatus.Success;
        }

        if (_agent.velocity.magnitude < 0.05f)
        {
            if (stuckTimer > 0f)
            {
                stuckTimer -= Time.deltaTime;
            }
            if (stuckTimer <= 0f)
            {
                return TaskStatus.Failed;
            }
        }
        else
        {
            stuckTimer = maxStuckTimer;
        }
        return TaskStatus.Running;
    }
}
