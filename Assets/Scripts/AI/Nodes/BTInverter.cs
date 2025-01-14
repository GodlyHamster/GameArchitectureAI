public class BTInverter : BTBaseNode
{
    BTBaseNode _node;

    public BTInverter(BTBaseNode nodeToInvert)
    {
        _node = nodeToInvert;
    }

    protected override TaskStatus OnUpdate()
    {
        TaskStatus status = _node.Tick();
        switch (status)
        {
            case TaskStatus.Success:
                return TaskStatus.Failed;
            case TaskStatus.Failed:
                return TaskStatus.Success;
            default:
                return TaskStatus.Running;
        }
    }

    public override void SetupBlackboard(Blackboard blackboard)
    {
        _node.SetupBlackboard(blackboard);
    }
}

