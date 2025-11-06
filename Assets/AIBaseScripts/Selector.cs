using UnityEngine;

public class Selector : Node
{
    Node[] children;
    int index;

    public Selector(Node[] Children, Condition[] conditions, BehaviorTree BT) : base(conditions, BT)
    {
        this.children = Children;
        foreach (Node child in Children)
        {
            child.SetParent(this);
        }
    }

    public override void EvaluateAction()
    {
        base.EvaluateAction();

        index = 0;
        children[index].EvaluateAction();
    }

    public override void FinishAction(bool result)
    {
        if (result)
        {
            base.FinishAction(true);
            return;
        }
        else if (index == children.Length - 1)
        {
            base.FinishAction(false);
        }
        else
        {
            index++;
            children[index].EvaluateAction();
        }
    }
}
