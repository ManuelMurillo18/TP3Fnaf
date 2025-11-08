using UnityEngine;

public class AttackCooldownCondition : Condition
{
    private readonly float cooldownTime;
    private readonly float triggerDuration = 1.5f;  
    private float nextReadyTime;
    private float triggerEndTime;
    private bool isTriggering; // are we in the "true" window?

    public AttackCooldownCondition(float cooldownTime, bool reverseCondition = false)
    {
        this.cooldownTime = cooldownTime;
       
        this.reverseCondition = reverseCondition;

        nextReadyTime = Time.time + cooldownTime;
        isTriggering = false;

        Debug.Log($"[Cooldown] Initialized → Ready at {nextReadyTime:F2}, stays TRUE for {triggerDuration:F1}s");
    }

    public override bool Evalutate()
    {
        float currentTime = Time.time;

        // If currently in the "true" window
        if (isTriggering)
        {
            if (currentTime <= triggerEndTime)
            {
                Debug.Log($"[Cooldown] TRUE window active ({triggerEndTime - currentTime:F2}s left)");
                return CheckForReverseCondition(true);
            }
            else
            {
                // Window expired → restart cooldown
                isTriggering = false;
                nextReadyTime = currentTime + cooldownTime;
                Debug.Log($"[Cooldown] TRUE window ended → Next ready at {nextReadyTime:F2}");
            }
        }

        // If cooldown expired → start TRUE window
        if (currentTime >= nextReadyTime && !isTriggering)
        {
            isTriggering = true;
            triggerEndTime = currentTime + triggerDuration;
            Debug.Log($"[Cooldown] Triggering TRUE window for {triggerDuration:F1}s");
            return CheckForReverseCondition(true);
        }

        // Still cooling down
        Debug.Log($"[Cooldown] Cooling down... {nextReadyTime - currentTime:F2}s left");
        return CheckForReverseCondition(false);
    }
}
