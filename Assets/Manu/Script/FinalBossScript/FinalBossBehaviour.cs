using UnityEngine;
using UnityEngine.AI;

public class FinalBossBehaviour : BehaviorTree
{
    [SerializeField] GameObject playerTarget;
    [SerializeField] Transform[] mouvmentTargets;
    [SerializeField] float roamingRange = 20f;
    Interrupt interrupt;

   
    
    protected override void InitializeTree()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        //************************************* Conditions *************************************//
        Condition[] seesPlayer = { new HasVision(agent.transform, playerTarget, 90, false) };
        //************************************* Interrupt *************************************//
        interrupt = new Interrupt(this, seesPlayer);

        FixedTargetMouvement movementTarget1 = new FixedTargetMouvement(agent, mouvmentTargets[0], 5, null, this);
        FixedTargetMouvement movementTarget2 = new FixedTargetMouvement(agent, mouvmentTargets[1], 5, null, this);
        FixedTargetMouvement movementTarget3 = new FixedTargetMouvement(agent, mouvmentTargets[2], 5, null, this);
        FixedTargetMouvement movementTarget4 = new FixedTargetMouvement(agent, mouvmentTargets[3], 5, null, this);
        FixedTargetMouvement movementTarget5 = new FixedTargetMouvement(agent, mouvmentTargets[4], 5, null, this);
        Chase chasePlayer = new Chase(null, this, agent, playerTarget, 7f);

        Sequence walkingSequence = new Sequence(new Node[] { movementTarget1, movementTarget2, movementTarget3, movementTarget4, movementTarget5 }, null, this);
        Selector chaseSelector = new Selector(new Node[] { chasePlayer}, seesPlayer, this);
        //Wandering wandering = new Wandering(null, this,agent,roamingRange);

        root = new Selector(new Node[] {chasePlayer,walkingSequence,}, null, this);
    }

    private void OnDisable()
    {
        if (interrupt != null)
            interrupt.Stop();
    }

    private void OnEnable()
    {
        if (interrupt != null)
            interrupt.Start();
    }
}
