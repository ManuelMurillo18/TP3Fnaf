using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : Node
{
    GameObject player;
    Transform target;
    NavMeshAgent agent;
    float stoppingDistance;

    public FollowPlayer(GameObject player, Transform target, NavMeshAgent agent, float stoppingDistance, Condition[] conditions, BehaviorTree BT)
    {
        this.player = player;
        this.target = target;
        this.agent = agent;
        this.stoppingDistance = stoppingDistance;
    }

    public override void EvaluateAction()
    {
        base.EvaluateAction();
        target = player.transform;
    }

    public override void Tick(float deltaTime)
    {
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
