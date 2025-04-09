using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BTThrowSmokeBomb : BTBaseNode
{
    private string _BBthrowLocation;
    private Vector3 _throwLocation;
    private SmokeBomb _smokeBomb;

    public BTThrowSmokeBomb(string throwLocation, SmokeBomb smokeBombObject)
    {
        _BBthrowLocation = throwLocation;
        _smokeBomb = smokeBombObject;
    }

    protected override void OnEnter()
    {
        _throwLocation = blackboard.GetVariable<Vector3>(_BBthrowLocation);
        _smokeBomb.transform.position = _throwLocation;
    }

    protected override TaskStatus OnUpdate()
    {
        if (_smokeBomb.IsActive) return TaskStatus.Failed;
        _smokeBomb.Throw();
        blackboard.SetVariable("smokeBombCooldown", 10f);
        return TaskStatus.Success;
    }
}

