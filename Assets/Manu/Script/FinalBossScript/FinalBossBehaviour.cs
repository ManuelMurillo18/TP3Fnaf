using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class FinalBossBehaviour : BehaviorTree
{
    [SerializeField]  GameObject playerTarget;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform[] mouvmentTargets;
    [SerializeField] Transform releasePoint;
    [SerializeField] float roamingRange = 20f;
    [SerializeField] Transform projectileReleasePoint;
    [SerializeField] float attackDuration = 5f;
    [SerializeField] float attackCooldown = 20f;
    [SerializeField] GameObject[] friendsPrefab;
    [SerializeField] Transform[] friendsSpawnPoint;
    Interrupt interrupt;



    protected override void InitializeTree()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Animator animator = GetComponent<Animator>();
        ManuPlayerComp playerComp = playerTarget.GetComponent<ManuPlayerComp>();
        //************************************* Conditions *************************************//
        Condition seesPlayer =  new HasVision(agent.transform, playerTarget, 45, false) ;
        Condition attackCooldownCondition =  new AttackCooldownCondition(attackCooldown, false) ;
        ////************************************* Interrupt *************************************//
        interrupt = new Interrupt(this, new Condition[] {seesPlayer,attackCooldownCondition});
        //interrupt = new Interrupt(this, attackCooldown);
        //interrupt = new Behavior_Interupt(this, seesPlayer);

        FixedTargetMouvement movementTarget1 = new FixedTargetMouvement(agent, mouvmentTargets[0], 5, null, this);
        FixedTargetMouvement movementTarget2 = new FixedTargetMouvement(agent, mouvmentTargets[1], 5, null, this);
        FixedTargetMouvement movementTarget3 = new FixedTargetMouvement(agent, mouvmentTargets[2], 5, null, this);
        FixedTargetMouvement movementTarget4 = new FixedTargetMouvement(agent, mouvmentTargets[3], 5, null, this);
        FixedTargetMouvement movementTarget5 = new FixedTargetMouvement(agent, mouvmentTargets[4], 5, null, this);
        Chase chasePlayer = new Chase(null, this, agent, playerTarget, 10f, 5f);
        NormalAttack normalAttack = new NormalAttack(animator,agent,this.transform,playerTarget, releasePoint,projectilePrefab, 40f,10f,playerTarget.tag,this, null);
        UltimateAttack ultimateAttack = new UltimateAttack(playerComp,animator,agent,friendsPrefab,friendsSpawnPoint,10f,interrupt,null,this);
        Sequence walkingSequence = new Sequence(new Node[] { movementTarget1, movementTarget2, movementTarget3, movementTarget4, movementTarget5 }, null, this);
        Sequence chaseSequence = new Sequence(new Node[] { chasePlayer, normalAttack}, new Condition[] { seesPlayer }, this);
        Sequence ultimateSequence = new Sequence(new Node[] { ultimateAttack },new Condition[] { attackCooldownCondition },this);

       

        //Wandering wandering = new Wandering(null, this,agent,roamingRange);

        root = new Selector(new Node[] {ultimateSequence,chaseSequence, walkingSequence}, null, this);
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




