using UnityEngine;
using UnityEngine.AI;

public class Chase : Node
{
    NavMeshAgent agent;
    GameObject target;
    float stopDistance;
    public Chase(Condition[] conditions,BehaviorTree BT, NavMeshAgent agent, GameObject target, float stopDistance) : base(conditions, BT)
    {
        this.agent = agent;
        this.target = target;
        this.stopDistance = stopDistance;
    }

    public override void Tick(float deltaTime)
    {
        if ((agent.destination - agent.transform.position).magnitude < stopDistance + 1.5f)
        {
            agent.SetDestination((agent.transform.position - agent.destination).normalized * stopDistance + agent.destination);
            Debug.Log("Arrived to chase target");
            FinishAction(true);
        }
        else
            agent.SetDestination(target.transform.position);
    }

    override public void EvaluateAction()
    {
        base.EvaluateAction();
        agent.SetDestination(target.transform.position);
    }


    public override void Interupt()
    {
        agent.destination = agent.transform.position;
        base.Interupt();
    }
}
