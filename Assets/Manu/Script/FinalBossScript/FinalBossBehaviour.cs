using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class FinalBossBehaviour : BehaviorTree
{
    [SerializeField] GameObject playerTarget;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform[] mouvmentTargets;
    [SerializeField] Transform releasePoint;
    [SerializeField] GameObject projectilePrefab;
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

<<<<<<< Updated upstream
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
=======
        FixedTargetMouvement movementTarget1 = new FixedTargetMouvement(agent, mouvmentTargets[0], 5, null, this);
        FixedTargetMouvement movementTarget2 = new FixedTargetMouvement(agent, mouvmentTargets[1], 5, null, this);
        FixedTargetMouvement movementTarget3 = new FixedTargetMouvement(agent, mouvmentTargets[2], 5, null, this);
        FixedTargetMouvement movementTarget4 = new FixedTargetMouvement(agent, mouvmentTargets[3], 5, null, this);
        FixedTargetMouvement movementTarget5 = new FixedTargetMouvement(agent, mouvmentTargets[4], 5, null, this);
        Chase chasePlayer = new Chase(null, this, agent, playerTarget, 7f);
        NormalAttack normalAttack = new NormalAttack(playerTarget, releasePoint,projectilePrefab, 40f, 0.5f, 5f, this, null);

        Sequence walkingSequence = new Sequence(new Node[] { movementTarget1, movementTarget2, movementTarget3, movementTarget4, movementTarget5 }, null, this);
        Sequence chaseSequence = new Sequence(new Node[] { chasePlayer, normalAttack}, seesPlayer, this);
        //Wandering wandering = new Wandering(null, this,agent,roamingRange);

        root = new Selector(new Node[] {chaseSequence,walkingSequence,}, null, this);
>>>>>>> Stashed changes
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




