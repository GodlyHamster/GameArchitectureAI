using UnityEngine;

public class BTDebug : BTBaseNode
{
    private string _debugText;

    public BTDebug(string debugText)
    {
        _debugText = debugText;
    }

    protected override TaskStatus OnUpdate()
    {
        Debug.Log(_debugText);
        return TaskStatus.Success;
    }
}

