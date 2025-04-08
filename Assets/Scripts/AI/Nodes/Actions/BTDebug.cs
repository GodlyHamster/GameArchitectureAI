using System;
using UnityEngine;

public class BTDebug : BTBaseNode
{
    private string _debugText;
    private Func<string> _funcString;

    public BTDebug(string debugText)
    {
        _debugText = debugText;
    }

    public BTDebug(Func<string> funcString)
    {
        _funcString = funcString;
    }

    protected override TaskStatus OnUpdate()
    {
        if (_funcString != null)
        {
            _debugText = _funcString();
        }
        if (_debugText != null && _debugText != "")
        {
            Debug.Log(_debugText);
            return TaskStatus.Success;
        }
        Debug.LogWarning("Failed to successfully log variable or text");
        return TaskStatus.Failed;
    }
}

