using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Wandering : Node
{
    float roamingRange;
    NavMeshAgent agent;

    public Wandering(Condition[] conditions, BehaviorTree BT,NavMeshAgent agent, float roamingRange) : base(conditions, BT)
    {
        this.agent = agent;
        this.roamingRange = roamingRange;
    }

    public override void Tick(float deltaTIme)
    {
        if ((agent.gameObject.transform.position - agent.destination).magnitude <= .75f)
        {

            FinishAction(true);
            return;
        }
    }
    override public void EvaluateAction()
    {
        base.EvaluateAction();
        Debug.Log("Started Wandering");
        Vector3 direction;
        do
        {
            direction = new Vector3(Random.Range(-roamingRange, roamingRange), 0, Random.Range(-roamingRange, roamingRange));
        }
        while (Physics.CapsuleCast(agent.transform.position + new Vector3(0, agent.height / 2, 0), agent.transform.position - new Vector3(0, agent.height / 2, 0), 2, direction));
        agent.destination = agent.transform.position + direction;
    }

    public override void FinishAction(bool result)
    {
        Debug.Log("Finished Wandering");
        agent.SetDestination(agent.transform.position);
        base.FinishAction(result);
    }

    public override void Interupt()
    {
        agent.destination = agent.transform.position;
        base.Interupt();
    }
}
