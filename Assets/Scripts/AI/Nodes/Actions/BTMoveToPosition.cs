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

    public BTMoveToPosition(NavMeshAgent agent, Vector3 targetPosition)
    {
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
        if (_agent.pathPending) { return TaskStatus.Success; }
        if (_agent.hasPath && _agent.path.status == NavMeshPathStatus.PathPartial) { return TaskStatus.Failed; }

        //checks if agent is stuck for too long and returns fail if true
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
        return TaskStatus.Success;
    }
}
