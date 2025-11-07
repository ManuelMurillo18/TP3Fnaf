using UnityEngine;

public class AttackCooldownCondition : Condition
{
    private AttackCooldown attackCooldown;

    public AttackCooldownCondition(AttackCooldown attackCooldown, bool reverseCondition = false)
    {
        this.attackCooldown = attackCooldown;
        this.reverseCondition = reverseCondition;
    }

    public override bool Evalutate()
    {
        bool canAttack = attackCooldown.canAttack;
        return CheckForReverseCondition(canAttack);
    }


}
