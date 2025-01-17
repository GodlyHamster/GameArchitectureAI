using System;
using UnityEngine;

public class BTThrowSmokeBomb : BTBaseNode
{
    private string _BBthrowLocation;
    private Vector3 _throwLocation;
    private GameObject _smokeBombObject;

    private float _smokeBombScale;
    private float _smokeBombMaxTime = 7f;
    private float _smokeBombTime = 0f;

    public BTThrowSmokeBomb(string throwLocation, GameObject smokeBombObject)
    {
        _BBthrowLocation = throwLocation;
        _smokeBombObject = smokeBombObject;
        _smokeBombObject.SetActive(false);
        _smokeBombScale = _smokeBombObject.transform.localScale.x;
    }

    protected override void OnEnter()
    {
        _throwLocation = blackboard.GetVariable<Vector3>(_BBthrowLocation);
        _smokeBombObject.transform.position = _throwLocation;
        _smokeBombTime = 0f;
        _smokeBombObject.SetActive(true);
        blackboard.SetVariable("smokeBombActive", true);
    }

    protected override TaskStatus OnUpdate()
    {
        if (_smokeBombTime < _smokeBombMaxTime)
        {
            _smokeBombTime += Time.deltaTime;
            _smokeBombObject.transform.localScale = _smokeBombTime <= 1f ? Vector3.one * (_smokeBombTime * _smokeBombScale) : Vector3.one * _smokeBombScale;
            return TaskStatus.Running;
        }
        _smokeBombObject.SetActive(false);
        blackboard.SetVariable("smokeBombActive", false);
        return TaskStatus.Success;
    }
}

