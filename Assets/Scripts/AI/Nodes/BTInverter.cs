public class BTInverter : BTBaseNode
{
    BTBaseNode _node;

    public BTInverter(BTBaseNode nodeToInvert)
    {
        _node = nodeToInvert;
    }

    protected override TaskStatus OnUpdate()
    {
        switch (_node.Tick())
        {
            case TaskStatus.Success:
                return TaskStatus.Failed;
            case TaskStatus.Failed:
                return TaskStatus.Success;
            default:
                return TaskStatus.Running;
        }
    }
}

