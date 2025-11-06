using UnityEngine;

public abstract class Condition
{
    protected bool reverseCondition;
    abstract public bool Evalutate();

    public bool CheckForReverseCondition(bool result)
    {
        if(reverseCondition)
            return !result;
        return result;
    }

}
