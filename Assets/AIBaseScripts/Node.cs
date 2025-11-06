using System;
using System.Diagnostics;

public abstract class Node
{
    protected Node parent;
    protected BehaviorTree BT;
    protected Condition[] conditions;

    public Node() { }
    public Node (Condition[] conditions, BehaviorTree BT)
    {
        this.BT = BT;
        this.conditions = conditions;
    }

    public void SetParent(Node parent)
    {
        this.parent = parent;
    }
    public bool EvalutateConditions()
    {
        if (conditions == null)
            return true;
        foreach (Condition condition in conditions)
        {
            if(!condition.Evaluate())
                return false;
        }
        return true;
    }
    virtual public void EvaluateAction()
    {
        
        if (!EvalutateConditions())
        {
            FinishAction(true);
        }

        BT.activeNode = this;
    }

    virtual public void Tick(float deltaTIme) { }
    virtual public void FinishAction(bool result)
    {
        if (parent != null)
            parent.FinishAction(result);
        else
            BT.EvaluateTree();
    }
    virtual public void Interupt()
    {
        if(parent != null)
        {
            parent.Interupt();
        }
    }
}
