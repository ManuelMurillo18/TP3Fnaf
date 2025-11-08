using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalBossBehaviour : BehaviorTree
{
    [SerializeField]  GameObject playerTarget;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform[] mouvmentTargets;
    [SerializeField] Transform releasePoint;
    [SerializeField] float roamingRange = 20f;
    [SerializeField] float attackDuration = 5f;
    [SerializeField] float attackCooldown = 20f;
    [SerializeField] GameObject[] friendsPrefab;
    [SerializeField] Transform[] friendsSpawnPoint;
    [SerializeField] float health;
    [SerializeField] Slider monsterHealthBar;
    Interrupt interrupt;
    private float initialHealthBarMaxValue;



    protected override void InitializeTree()
    {
        initialHealthBarMaxValue = health;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Animator animator = GetComponent<Animator>();
        ManuPlayerComp playerComp = playerTarget.GetComponent<ManuPlayerComp>();
        //************************************* Conditions *************************************//
        Condition seesPlayer =  new HasVision(agent.transform, playerTarget, 90, false) ;
        Condition attackCooldownCondition =  new AttackCooldownCondition(attackCooldown, false) ;
        ////************************************* Interrupt *************************************//
        interrupt = new Interrupt(this, new Condition[] {seesPlayer,attackCooldownCondition});
       
        Wait waitAfterAttack = new Wait(2f, null, this);

        FixedTargetMouvement movementTarget1 = new FixedTargetMouvement(agent, mouvmentTargets[0], 5, null, this);
        FixedTargetMouvement movementTarget2 = new FixedTargetMouvement(agent, mouvmentTargets[1], 5, null, this);
        FixedTargetMouvement movementTarget3 = new FixedTargetMouvement(agent, mouvmentTargets[2], 5, null, this);
        FixedTargetMouvement movementTarget4 = new FixedTargetMouvement(agent, mouvmentTargets[3], 5, null, this);
        FixedTargetMouvement movementTarget5 = new FixedTargetMouvement(agent, mouvmentTargets[4], 5, null, this);
        Chase chasePlayer = new Chase(null, this, agent, playerTarget, 10f, 5f);
        NormalAttack normalAttack = new NormalAttack(animator,agent,this.transform,playerTarget, releasePoint,projectilePrefab, 2f,10f,playerTarget.tag, interrupt,this, null);
        UltimateAttack ultimateAttack = new UltimateAttack(playerComp,animator,agent,friendsPrefab,friendsSpawnPoint,10f,interrupt,null,this);
        Sequence walkingSequence = new Sequence(new Node[] { movementTarget1, movementTarget2, movementTarget3, movementTarget4, movementTarget5 }, null, this);
        Sequence chaseSequence = new Sequence(new Node[] { chasePlayer, normalAttack,waitAfterAttack}, new Condition[] { seesPlayer }, this);
        Sequence ultimateSequence = new Sequence(new Node[] { ultimateAttack },new Condition[] { attackCooldownCondition },this);

        //Wandering wandering = new Wandering(null, this,agent,roamingRange);

        root = new Selector(new Node[] {ultimateSequence,chaseSequence, walkingSequence}, null, this);
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

    public void TakeDamage(int damage)
    {
        Debug.Log($"You inflicted this number of damage : {damage} ");
        health -= damage;
        monsterHealthBar.value = health / initialHealthBarMaxValue;
        if (health <= 0)
        {
            SceneManager.LoadScene("Win", LoadSceneMode.Single);
        }
    
    }

}




