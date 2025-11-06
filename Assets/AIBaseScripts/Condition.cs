using UnityEngine;

public abstract class Condition
{
    protected bool reverseCondition;
    abstract public bool Evaluate();

    public bool CheckForReverseCondition(bool result)
    {
        if(reverseCondition)
            return !result;
        return result;
    }

}
