using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTUpdateVariable<T> : BTBaseNode
{
    private T Value;
    private string BBvariable;
    private Func<T> FuncValue;

    public BTUpdateVariable(string BBvariable, T newValue)
    {
        this.BBvariable = BBvariable;
        Value = newValue;
    }

    public BTUpdateVariable(string BBvariable, Func<T> newFuncValue)
    {
        this.BBvariable = BBvariable;
        FuncValue = newFuncValue;
    }

    protected override void OnEnter()
    {
        if (FuncValue != null)
        {
            Value = FuncValue();
        }
    }

    protected override TaskStatus OnUpdate()
    {
        blackboard.SetVariable(BBvariable, Value);
        return TaskStatus.Success;
    }
}
