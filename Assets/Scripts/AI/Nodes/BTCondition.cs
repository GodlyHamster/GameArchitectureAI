using System;
using UnityEngine;

public class BTCondition : BTBaseNode
{
    private bool _condition;
    private string _BBCondition;
    private Func<bool> _conditionPredicate;

    public BTCondition(bool condition)
    {
        _condition = condition;
    }

    public BTCondition(string condition)
    {
        _BBCondition = condition;
    }

    public BTCondition(Func<bool> condition)
    {
        _conditionPredicate = condition;
    }

    protected override void OnEnter()
    {
        if (_conditionPredicate != null)
        {
            _condition = _conditionPredicate();
            return;
        }
        if (_BBCondition != null && _BBCondition != "")
        {
            _condition = blackboard.GetVariable<bool>(_BBCondition);
            return;
        }
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
