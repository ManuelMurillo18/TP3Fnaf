using UnityEngine;
using UnityEngine.AI;

public class GoToTarget : Node
{
    Transform target;
    float stoppingDistance;
    NavMeshAgent agent;
    PatrolAnimatronicComponent animatronic;


    public GoToTarget(NavMeshAgent agent,PatrolAnimatronicComponent animatronic , Transform target, float stoppingDistance, Condition[] conditions, BehaviorTree BT) : base(conditions, BT)
    {
        this.agent = agent;
        this.animatronic = animatronic;
        this.target = target;
        this.stoppingDistance = stoppingDistance;
    }

    public override void EvaluateAction()
    {
        base.EvaluateAction();
        agent.stoppingDistance = 0;
        agent.SetDestination(target.position);
    }

    public override void Tick(float deltaTime)
    {
        animatronic.SetCanRun(false);
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
