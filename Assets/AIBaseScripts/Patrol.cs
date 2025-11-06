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

        //************************************* Conditions *************************************//
        Condition[] doesntSeePlayer = { new HasVision(agent.transform, player, 90, true) };
        Condition[] seesPlayer = { new HasVision(agent.transform, player, 90, false) };


        //************************************* Interrupt *************************************//
        //interrupt = new Interrupt(this, seesPlayer);


        //************************************* Nodes *************************************//
        GoToTarget goTo1 = new GoToTarget(agent, targets[0], 2, null, this);
        GoToTarget goTo2 = new GoToTarget(agent, targets[1], 2, null, this);
        GoToTarget goTo3 = new GoToTarget(agent, targets[2], 2, null, this);
        GoToTarget goTo4 = new GoToTarget(agent, targets[3], 2, null, this);
        Wait wait2 = new Wait(2, null, this);
        Wait wait4 = new Wait(4, null, this);

        FollowPlayer followPlayer = new FollowPlayer(player, player.transform, agent, 2, null, this);


        //*************************************** Sequences *************************************//
        Sequence patrolSequence = new Sequence(new Node[] { goTo1, wait2, goTo2, wait2, goTo3, wait2, goTo4, wait2 }, doesntSeePlayer, this);
        Sequence waitSequence = new Sequence(new Node[] { wait4 }, null, this);
        //Sequence followPlayerSequence = new Sequence(new Node[] { followPlayer, wait4 }, seesPlayer, this);


        //*************************************** Root Node *************************************//
        root = new Sequence(new Sequence[] { patrolSequence, waitSequence }, null, this);
    }

    //private void OnDisable()
    //{
    //    interrupt.Stop();
    //}

    //private void OnEnable()
    //{
    //    if (interrupt != null)
    //        interrupt.Start();
    //}
}
