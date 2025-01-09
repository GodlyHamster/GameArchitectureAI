using UnityEngine;

public class BTWait : BTBaseNode
{
    private float _maxWaitTime = 0f;
    private float _waitTime = 0f;

    public BTWait(float waitTime)
    {
        _maxWaitTime = waitTime;
    }

    protected override void OnEnter()
    {
        _waitTime = _maxWaitTime;
    }

    protected override TaskStatus OnUpdate()
    {
        if (_waitTime > 0f)
        {
            _waitTime -= Time.deltaTime;
            if (_waitTime > 0f) return TaskStatus.Running;
        }
        return TaskStatus.Success;
    }
}
