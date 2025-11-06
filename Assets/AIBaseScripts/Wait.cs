using UnityEngine;

public class Wait : Node
{
    float secondsToWait;
    float timer;

    public Wait(float secondsToWait, Condition[] conditions, BehaviorTree BT ) : base(conditions, BT)
    {
        this.secondsToWait = secondsToWait;
    }

    public override void EvaluateAction()
    {
        timer = 0;
        base.EvaluateAction();
    }

    public override void Tick(float deltaTIme)
    {
        timer += deltaTIme;
        if (timer >= secondsToWait)
        {
            FinishAction(true);
        }
    }

}
