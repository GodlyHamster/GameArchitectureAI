using UnityEngine;
using UnityEngine.AI;

public class BTGrabWeapon : BTBaseNode
{
    private NavMeshAgent _agent;
    private string _BBHasWeapon;

    public BTGrabWeapon(NavMeshAgent agent, string varToUpdate)
    {
        _agent = agent;
        _BBHasWeapon = varToUpdate;
    }

    protected override TaskStatus OnUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(_agent.transform.position, 3f);
        foreach (Collider item in colliders)
        {
            if (item.gameObject.tag != "Weapon") continue;
            blackboard.SetVariable(_BBHasWeapon, true);
            return TaskStatus.Success;
        }
        return TaskStatus.Failed;
    }
}

