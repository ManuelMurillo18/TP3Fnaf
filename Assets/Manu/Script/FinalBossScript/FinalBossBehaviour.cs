using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class FinalBossBehaviour : BehaviorTree
{
    [SerializeField] GameObject playerTarget;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform[] mouvmentTargets;
    [SerializeField] float roamingRange = 20f;
    [SerializeField] Transform projectileReleasePoint;
    [SerializeField] float attackDuration = 5f;
   // [SerializeField] AttackCooldown attackCooldownComponent;
    Interrupt interrupt;



    protected override void InitializeTree()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        //************************************* Conditions *************************************//
        Condition[] seesPlayer = { new HasVision(agent.transform, playerTarget, 90, false) };
       //Condition[] attackCooldown = { new AttackCooldownCondition(attackCooldownComponent, false) };
        ////************************************* Interrupt *************************************//
        interrupt = new Interrupt(this, seesPlayer);
        //interrupt = new Interrupt(this, attackCooldown);

        FixedTargetMouvement firstMove = new FixedTargetMouvement(agent, mouvmentTargets[0], 5, null, this);
        FixedTargetMouvement secondMove = new FixedTargetMouvement(agent, mouvmentTargets[1], 5, null, this);
        FixedTargetMouvement thirdMove = new FixedTargetMouvement(agent, mouvmentTargets[2], 5, null, this);
        FixedTargetMouvement fourthMove = new FixedTargetMouvement(agent, mouvmentTargets[3], 5, null, this);
        FixedTargetMouvement fifthMove = new FixedTargetMouvement(agent, mouvmentTargets[4], 5, null, this);
        //Wait waitBetweenMoves = new Wait(20f, null, this);
        //TimedAttack attackAction = new TimedAttack(projectilePrefab, playerTarget.transform, projectileReleasePoint, attackDuration, null, this);
        Chase chase = new Chase(null, this, agent,playerTarget,5);


        Sequence movement = new Sequence(new Node[] { firstMove, secondMove, thirdMove, fourthMove, fifthMove }, null, this);
        Selector attack = new Selector(new Node[] { chase }, seesPlayer, this);

        // Parallel now automatically restarts both forever
        root = new Selector(new Node[] { attack, movement }, null, this);
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




