using UnityEngine;

public class TimedAttack : Node
{
    Transform target;
    Transform releasePoint;
    GameObject projectile;
   // AttackCooldown attackCooldown;
    float attackDuration;
    float attackSpeed = 0.2f;
    float attackTimer = 0f;
    float elapsedTime = 0f;

    public TimedAttack(GameObject projectile,Transform target, Transform releasePoint, float attackDuration, Condition[] condition, BehaviorTree BT) : base(condition, BT)
    {
        //this.attackCooldown = attackCooldown;
        this.projectile = projectile;
        this.target = target;
        this.releasePoint = releasePoint;
        this.attackDuration = attackDuration;
    }

    public override void EvaluateAction()
    {
        // Start attack animation or logic here
        Debug.Log("Starting Timed Attack on " + target.name + " from " + releasePoint.name + " for " + attackDuration + " seconds.");
        elapsedTime = 0f;
        attackTimer = 0f;
        base.EvaluateAction();
    }

    public override void Tick(float deltaTIme)
    {
        elapsedTime += deltaTIme;
        attackTimer += deltaTIme;
        if (attackTimer >= attackSpeed)
        {
            FireProjectile();
            attackTimer = 0f;
        }
        if (elapsedTime >= attackDuration)
        {
            // Finish attack logic here
            Debug.Log("Finished Timed Attack on " + target.name);
           // attackCooldown.Attacked();
            FinishAction(true);
        }
    }

    public void FireProjectile()
    {
        
        GameObject proj = GameObject.Instantiate(projectile, releasePoint.position, Quaternion.identity);
        Vector3 direction = (target.position - releasePoint.position).normalized;
        proj.GetComponent<Rigidbody>().linearVelocity = direction * 20f; // Example speed
        Debug.Log("Fired projectile towards " + target.name);
    }
}
