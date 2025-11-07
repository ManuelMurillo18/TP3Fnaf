using UnityEngine;

public class AttackJumpScare : Node
{
    GameObject player;
    PatrolAnimatronicComponent patrolComponent;

    public AttackJumpScare(GameObject player, PatrolAnimatronicComponent patrolAnimatronic,  Condition[] conditions, BehaviorTree BT) : base(conditions, BT)
    {
        this.player = player;
        this.patrolComponent = patrolAnimatronic;
    }

    public override void Tick(float deltaTime)
    {
        patrolComponent.JumpScare(player);
        FinishAction(true);
    }
}
