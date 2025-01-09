using UnityEngine;
using UnityEngine.AI;

public class BTMoveToPosition : BTBaseNode
{
    private NavMeshAgent _agent;
    private string _BBTargetPosition;
    private Vector3 _targetPosition;

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
        if (_agent.hasPath && _agent.path.status == NavMeshPathStatus.PathInvalid) { return TaskStatus.Failed; }
        if (_agent.pathEndPosition != _targetPosition)
        {
            _agent.SetDestination(_targetPosition);
        }

        if (Vector3.Distance(_agent.transform.position, _targetPosition) <= 0.3f)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
