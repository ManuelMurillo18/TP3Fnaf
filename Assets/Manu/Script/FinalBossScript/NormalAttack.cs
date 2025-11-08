using UnityEngine;
using UnityEngine.AI;

public class NormalAttack : Node
{
    Animator animator;
    NavMeshAgent agent;
    Transform self;
    GameObject player;
    Transform projectileReleasePoint;
    GameObject projectilePrefab;
    float projectileSpeed;
    float meleeRange = 5f; // distance threshold
    Vector3 meleeRangeVector = new Vector3(3f, 3f, 3f);
    bool sucess = false;
    string targetTag;
    Interrupt interrupt;
    bool hasAttacked = false;

    public NormalAttack(Animator animator, NavMeshAgent agent, Transform self, GameObject player, Transform projectileReleasePoint, GameObject projectilePrefab, float projectileSpeed, float meleeRange, string targetTag, Interrupt interrupt, BehaviorTree tree, Condition[] conditions) : base(conditions, tree)
    {
        this.animator = animator;
        this.agent = agent;
        this.self = self;
        this.player = player;
        this.projectileReleasePoint = projectileReleasePoint;
        this.projectilePrefab = projectilePrefab;
        this.projectileSpeed = projectileSpeed;
        this.meleeRange = meleeRange;
        this.targetTag = targetTag;
        this.interrupt = interrupt;

    }

    public override void EvaluateAction()
    {
        base.EvaluateAction();

    }

    public override void Tick(float deltaTime)
    {
        float distance = Vector3.Distance(self.position, player.transform.position);

        if (distance <= meleeRange)
        {
            DoMeleeAttack();
        }
        else
        {
            FireProjectile();
        }
    }

    void DoMeleeAttack()
    {
        
        interrupt.Stop();
        animator.SetTrigger("isAttacking");
        agent.isStopped = true;
        Vector3 direction = (player.transform.position - self.position).normalized;
        Vector3 originOffset = Vector3.up;
        Debug.Log("Performing melee attack");
        RaycastHit[] hits = Physics.BoxCastAll(self.position + originOffset, meleeRangeVector / 2, direction);
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.tag == targetTag)
                {
                    sucess = true;
                    break;
                }
            }
        }
        if (sucess)
        {
            ManuPlayerComp securityGuard = player.GetComponent<ManuPlayerComp>();
            if (securityGuard != null)
            {
                securityGuard.TakeDamage(Random.Range(1, 15));
            }
        }
        agent.isStopped = false;
        FinishAction(sucess);
        interrupt.Start();

    }

    void FireProjectile()
    {
        interrupt.Stop();
        for (int i = 0; i <= 10; i++)
        {
            GameObject proj = GameObject.Instantiate(projectilePrefab, projectileReleasePoint.position + agent.transform.forward, Quaternion.identity);
            Debug.Log("Firing projectile to player.");
            Vector3 direction = (player.transform.position - projectileReleasePoint.position).normalized;
            proj.transform.forward = direction;

            Rigidbody rb = proj.GetComponent<Rigidbody>();
            rb.linearVelocity = direction * projectileSpeed;
        }
        interrupt.Start();
        FinishAction(true);
    }

    public override void Interupt()
    {
        base.Interupt();
    }
    public void AfterAttack()
    {
        agent.isStopped = true;
        hasAttacked = true;
    }


}
