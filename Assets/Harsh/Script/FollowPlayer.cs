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
        agent.stoppingDistance = stoppingDistance;
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
                // pendant la pause, on arrête le mouvement
                agent.SetDestination(agent.transform.position);
                return;
            }
        }

        Debug.Log(Vector3.Distance(agent.transform.position, target.position));
        if (Vector3.Distance(agent.transform.position, target.position) <= stoppingDistance)
        {
            Debug.Log("Arrived to chase target");
            FinishAction(true);
        }
        else
        {
            agent.SetDestination(target.position);
        }
        //Debug.Log((agent.destination - agent.transform.position).magnitude);
        //if ((agent.destination - agent.transform.position).magnitude < stoppingDistance + 1.5f)
        //{
        //    agent.SetDestination((agent.transform.position - agent.destination).normalized * stoppingDistance + agent.destination);
        //    Debug.Log("Arrived to chase target");
        //    FinishAction(true);
        //}
        //else
        //    agent.SetDestination(target.transform.position);
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
