using UnityEngine;

public class Sequence : Node
{
    Node[] children;
    int index;
    public Sequence(Node[] Children, Condition[] conditions, BehaviorTree BT): base(conditions, BT)
    {
        this.children = Children;
        foreach (var item in Children)
        {
            item.SetParent(this);
        }
    }

    public override void EvaluateAction()
    {
        //base.EvaluateAction();
        index = 0;
        if (EvaluateConditions())
        {
            children[index].EvaluateAction();
        }
        else
        {
            FinishAction(false);
        }
    }


    public override void FinishAction(bool result)
    {
        if (!result)
        {
            base.FinishAction(false);
        }
        else if(index == children.Length - 1)
        {
            base.FinishAction(true);
        }
        else
        {
            index++;
            children[index].EvaluateAction();
        }
    }
}
