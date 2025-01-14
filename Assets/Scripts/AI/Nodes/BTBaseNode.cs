using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskStatus { Success, Running, Failed }
public interface NodeInterface
{
    BTBaseNode GetState();
}
public abstract class BTBaseNode : NodeInterface
{
    protected Blackboard blackboard;
    private bool wasEntered = false;

    public virtual void OnReset() { }

    public TaskStatus Tick()
    {
        if (!wasEntered)
        {
            OnEnter();
            wasEntered = true;
        }

        TaskStatus result = OnUpdate();
        if (result != TaskStatus.Running)
        {
            OnExit();
            wasEntered = false;
        }
        return result;
    }

    public virtual void SetupBlackboard(Blackboard blackboard)
    {
        this.blackboard = blackboard;
    }


    protected abstract TaskStatus OnUpdate();
    protected virtual void OnEnter() { }
    protected virtual void OnExit() { }

    public virtual BTBaseNode GetState()
    {
        return this;
    }
}

public abstract class BTCompositeNode : BTBaseNode
{
    protected BTBaseNode[] children;
    public BTCompositeNode(params BTBaseNode[] children)
    {
        this.children = children;
    }

    public override void SetupBlackboard(Blackboard blackboard)
    {
        base.SetupBlackboard(blackboard);
        foreach (BTBaseNode node in children)
        {
            node.SetupBlackboard(blackboard);
        }
    }
}

public abstract class BTReturnNode<T> : BTBaseNode
{
    public abstract T ReturnValue();
}
