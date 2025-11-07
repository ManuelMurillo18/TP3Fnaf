using UnityEngine;

public class NormalAttack : Node
{
    GameObject player;
    Transform projectileReleasePoint;
    GameObject projectilePrefab;
    float projectileSpeed;
    float attackCooldown;
    float timer;
    float attackTimer;
    float attackDuration;

    public NormalAttack(GameObject player, Transform projectileReleasePoint, GameObject projectilePrefab,float projectileSpeed, float attackCooldown, float attackDuration,BehaviorTree tree, Condition[] conditions): base(conditions, tree)
    {
        this.player = player;
        this.projectileReleasePoint = projectileReleasePoint;
        this.projectilePrefab = projectilePrefab;
        this.projectileSpeed = projectileSpeed;
        this.attackCooldown = attackCooldown;
        this.attackDuration = attackDuration;
    }

    public override void EvaluateAction()
    {
        base.EvaluateAction();
        timer = 0f;
        attackTimer = 0f;    
        Debug.Log("Starting normal attack");
    }

    public override void Tick(float deltaTime)
    {
        timer += deltaTime;
        attackTimer += deltaTime;

        if (attackTimer >= attackCooldown)
        {
            FireProjectile();
            attackTimer = 0f;
        }

        if (timer >= attackDuration)
        {
            Debug.Log("Attack finished");
            FinishAction(true);
        }
    }

    void FireProjectile()
    {
        GameObject proj = GameObject.Instantiate(projectilePrefab, projectileReleasePoint.position, Quaternion.identity);
        Vector3 direction = (player.transform.position - projectileReleasePoint.position).normalized;
        proj.transform.forward = direction;
        proj.GetComponent<Rigidbody>().linearVelocity = direction * projectileSpeed;
        Debug.Log("Fired projectile toward player");
    }

    public override void Interupt()
    {
        timer = 0f;
        attackTimer = 0f;
        base.Interupt();
    }
}
