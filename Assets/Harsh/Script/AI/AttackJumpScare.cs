using UnityEngine;

public class AttackJumpScare : Node
{
    public AttackJumpScare(Condition[] conditions, BehaviorTree BT) : base(conditions, BT)
    {
        
    }

    public override void Tick(float deltaTime)
    {
        Debug.Log("Jumpscare Activated");
        JumpscareManager.Instance.ActivateJumpscare();
        FinishAction(true);
    }
}
