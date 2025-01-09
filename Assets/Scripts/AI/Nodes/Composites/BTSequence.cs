public class BTSequence : BTCompositeNode
{
    private int _currentIndex = 0;

    public BTSequence(params BTBaseNode[] children) : base(children) { }

    protected override TaskStatus OnUpdate()
    {
        for (; _currentIndex < children.Length; _currentIndex++)
        {
            TaskStatus result = children[_currentIndex].Tick();
            switch (result)
            {
                case TaskStatus.Success:
                    continue;
                case TaskStatus.Running:
                    return TaskStatus.Running;
                case TaskStatus.Failed:
                    return TaskStatus.Failed;
            }
        }
        return TaskStatus.Success;
    }

    public BTBaseNode GetCurrentState()
    {
        return children[_currentIndex];
    }

    protected override void OnEnter()
    {
        _currentIndex = 0;
    }

    protected override void OnExit()
    {
        _currentIndex = 0;
    }

    public override void OnReset()
    {
        _currentIndex = 0;
        foreach (BTBaseNode node in children)
        {
            node.OnReset();
        }
    }
}
