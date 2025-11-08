//using UnityEngine;
//using UnityEngine.AI;

//public class BadPatrol : BehaviorTree
//{
//    [SerializeField] Transform[] targets;
//    Interrupt interrupt;
//    protected override void InitializeTree()
//    {
//        NavMeshAgent agent = GetComponent<NavMeshAgent>();

//        //Interrupt interupt = new Interrupt(this, new Condition[] {new HasVision(gameObject.transform, gameObject, 15, false)});

//        GoToTarget goTo1 = new GoToTarget(agent, targets[0], 2, null, this);
//        Wait wait1 = new Wait(2, null, this);
//        GoToTarget goTo2 = new GoToTarget(agent, targets[1], 2, null, this);

//        root = new Sequence(new Node[] { goTo1, wait1, goTo2, wait1 }, null, this);
//    }

//    private void OnDisable()
//    {
//        interrupt.Stop();
//    }

//    private void OnEnable()
//    {
//        if (interrupt != null)
//            interrupt.Start();
//    }
//}
