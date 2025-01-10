using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTPatrol : BTBaseNode
{
    private NavMeshAgent _agent;
    private string _BBcurrentPoint;
    private LinkedListNode<Vector3> _targetPosition;

    private float maxStuckTimer = 3f;
    private float stuckTimer = 3f;

    public BTPatrol(NavMeshAgent agent, string currentPoint)
    {
        _agent = agent;
        _BBcurrentPoint = currentPoint;
    }

    protected override void OnEnter()
    {
        _targetPosition = blackboard.GetVariable<LinkedListNode<Vector3>>(_BBcurrentPoint);
    }

    protected override TaskStatus OnUpdate()
    {
        if (_agent == null) { return TaskStatus.Failed; }
        if (_agent.pathPending) { return TaskStatus.Running; }
        if (_agent.hasPath && _agent.path.status == NavMeshPathStatus.PathPartial)
        {
            blackboard.SetVariable(_BBcurrentPoint, _targetPosition.Next);
            return TaskStatus.Failed; 
        }
        if (_agent.pathEndPosition != _targetPosition.Value)
        {
            _agent.SetDestination(_targetPosition.Value);
        }

        if (Vector3.Distance(_agent.transform.position, _targetPosition.Value) <= 0.3f)
        {
            blackboard.SetVariable(_BBcurrentPoint, _targetPosition.Next);
            return TaskStatus.Success;
        }

        //checks if agent is stuck for too long and returns fail if true
        if (_agent.velocity.magnitude < 0.05f)
        {
            if (stuckTimer > 0f)
            {
                stuckTimer -= Time.deltaTime;
            }
            if (stuckTimer <= 0f)
            {
                blackboard.SetVariable(_BBcurrentPoint, _targetPosition.Next);
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
