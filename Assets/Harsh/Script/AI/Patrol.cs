using UnityEngine;
using UnityEngine.AI;

public class Patrol : BehaviorTree
{
    [SerializeField] Transform[] targets;
    Interrupt interrupt;
    protected override void InitializeTree()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        PatrolAnimatronicComponent animatronic = GetComponent<PatrolAnimatronicComponent>();

        //************************************* Conditions *************************************//
        Condition[] doesntSeePlayer = { new HasVision(agent.transform, player, 90f, true) };
        Condition[] seesPlayer = { new HasVision(agent.transform, player, 90f, false) };


        //************************************* Interrupt *************************************//
        interrupt = new Interrupt(this, seesPlayer, 3f);


        //************************************* Nodes *************************************//
        GoToTarget goTo1 = new GoToTarget(agent, animatronic, targets[0], 4f, null, this);
        GoToTarget goTo2 = new GoToTarget(agent, animatronic, targets[1], 4f, null, this);
        GoToTarget goTo3 = new GoToTarget(agent, animatronic, targets[2], 4f, null, this);
        GoToTarget goTo4 = new GoToTarget(agent, animatronic, targets[3], 4f, null, this);
        Wait wait2 = new Wait(2, null, this);
        Wait wait4 = new Wait(4, null, this);

        FollowPlayer followPlayer = new FollowPlayer(player, player.transform, agent, animatronic, 7f, 20f, 4f, null, this);
        AttackJumpScare attack = new AttackJumpScare(null, this);

        //*************************************** Sequences *************************************//
        Sequence patrolSequence = new Sequence(new Node[] { goTo1, wait2, goTo2, wait2, goTo3, wait2, goTo4, wait2 }, doesntSeePlayer, this);
        Sequence attackSequence = new Sequence(new Node[] { followPlayer, attack }, null, this);
        //Sequence chaseSequence = new Sequence(new Node[] { followPlayer }, null, this);
        //Sequence followPlayerSequence = new Sequence(new Node[] { followPlayer, wait4 }, seesPlayer, this);

        //*************************************** Root Node *************************************//
        root = new Selector(new Node[] { patrolSequence, attackSequence }, null, this);
    }

    private void OnDisable()
    {
        interrupt.Stop();
    }

    private void OnEnable()
    {
        if (interrupt != null)
            interrupt.Start();
    }
}
