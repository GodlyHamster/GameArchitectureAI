using System;
using UnityEngine;

public class BTCondition : BTBaseNode
{
    private string _BBCondition;
    private bool _condition;

    public BTCondition(bool condition)
    {
        _condition = condition;
    }

    public BTCondition(string condition)
    {
        _BBCondition = condition;
    }

    protected override void OnEnter()
    {
        if (_BBCondition == null) return;
        _condition = blackboard.GetVariable<bool>(_BBCondition);
    }

    protected override TaskStatus OnUpdate()
    {
        if (_condition)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failed;
    }
}
