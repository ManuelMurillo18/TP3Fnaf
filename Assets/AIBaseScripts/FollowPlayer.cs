using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : Node
{
    GameObject player;
    Transform target;
    NavMeshAgent agent;
    float stoppingDistance;
    float staminaDuration;
    float staminaRecoverDuration;

    float chasingTime;
    float recoverTime;
    bool isRecovering = false;

    public FollowPlayer(GameObject player, Transform target, NavMeshAgent agent, float stoppingDistance, float staminaDuration, float staminaRecoverDuration, Condition[] conditions, BehaviorTree BT) : base(conditions, BT)
    {
        this.player = player;
        this.target = target;
        this.agent = agent;
        this.stoppingDistance = stoppingDistance;
        this.staminaDuration = staminaDuration;
        this.staminaRecoverDuration = staminaRecoverDuration;
    }

    public override void EvaluateAction()
    {
        base.EvaluateAction();
        chasingTime = 0f;
        recoverTime = 0f;
        isRecovering = false;

        target = player.transform;
    }

    public override void Tick(float deltaTime)
    {
        chasingTime += deltaTime;

        if(chasingTime >= staminaDuration)
        {
            isRecovering = true;
        }

        if (isRecovering)
        {
            recoverTime += deltaTime;
            if (recoverTime >= staminaRecoverDuration)
            {
                isRecovering = false;
                chasingTime = 0f;
                recoverTime = 0f;
            }
            else
            {
                // While recovering, do not move
                agent.SetDestination(agent.transform.position);
                return;
            }
        }

        if ((agent.transform.position - target.position).sqrMagnitude < stoppingDistance * stoppingDistance)
        {
            FinishAction(true);
        }
        else
        {
            if (!agent.SetDestination(target.position))
            {
                FinishAction(false);
            }
        }
    }

    public override void FinishAction(bool result)
    {
        agent.SetDestination(agent.transform.position);
        base.FinishAction(result);
    }

    public override void Interupt()
    {
        agent.SetDestination(agent.transform.position);
        base.Interupt();
    }
}
