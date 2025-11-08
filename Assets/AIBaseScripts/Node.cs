using System;
using System.Diagnostics;
using UnityEngine;

public abstract class Node
{
    protected Node parent;
    public BehaviorTree BT;
    protected Condition[] conditions;
    protected bool interupted = false;

    public Node(Condition[] conditions, BehaviorTree BT)
    {
        this.BT = BT;
        this.conditions = conditions;
    }

    public Node()
    {
    }

    public void SetParent(Node parent)
    {
        this.parent = parent;
    }

    public bool EvaluateConditions()
    {
        if (conditions == null)
            return true;

        foreach (Condition c in conditions)
        {
            if (!c.Evaluate())
                return false;
        }
        return true;
    }

    virtual public void EvaluateAction()
    {
        if (!EvaluateConditions())
        {
            FinishAction(false);
        }

        BT.activeNode = this;
    }

    virtual public void Tick(float deltaTime)
    {

    }

    virtual public void FinishAction(bool result)
    {
        if (parent != null)
            parent.FinishAction(result);
        else
            BT.OnTreeFinished(); // instead of BT.EvaluateTree() 
    }

    virtual public void Interupt()
    {
        if (parent != null)
            parent.Interupt();
    }



}