using UnityEngine;
using UnityEngine.AI;

public class GoToTarget : Node
{
    Transform target;
    float stoppingDistance;
    NavMeshAgent agent;

    public GoToTarget(NavMeshAgent agent, Transform target, float stoppingDistance, Condition[] conditions, BehaviorTree BT) : base(conditions, BT)
    {
        this.agent = agent;
        this.target = target;
        this.stoppingDistance = stoppingDistance;
    }

    public override void EvaluateAction()
    {
        base.EvaluateAction();
        agent.SetDestination(target.position);
    }

    public override void Tick(float deltaTime)
    {
        if((agent.transform.position - target.position).sqrMagnitude < stoppingDistance * stoppingDistance)
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
