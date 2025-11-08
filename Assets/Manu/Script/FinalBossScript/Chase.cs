using UnityEngine;
using UnityEngine.AI;

public class Chase : Node
{
    NavMeshAgent agent;
    GameObject target;
    float stopDistance;
    float chaseDuration;
    float chaseTimer;

    public Chase(Condition[] conditions, BehaviorTree BT, NavMeshAgent agent, GameObject target, float stopDistance, float chaseDuration)
        : base(conditions, BT)
    {
        this.agent = agent;
        this.target = target;
        this.stopDistance = stopDistance;
        this.chaseDuration = chaseDuration;
    }

    public override void EvaluateAction()
    {
        base.EvaluateAction();
        chaseTimer = 0f;
        agent.SetDestination(target.transform.position);
        Debug.Log("Chase started.");
    }

    public override void Tick(float deltaTime)
    {
       
        chaseTimer += deltaTime;

       
        if (chaseTimer >= chaseDuration)
        {
            Debug.Log("Chase time over.");
            FinishAction(true); 
            return;
        }

        
        agent.SetDestination(target.transform.position);

        
        if ((agent.transform.position - target.transform.position).magnitude < stopDistance)
        {
            Debug.Log("Arrived at target during chase.");
            FinishAction(true);
        }
    }

    public override void Interupt()
    {
        agent.destination = agent.transform.position;
        base.Interupt();
    }
}
