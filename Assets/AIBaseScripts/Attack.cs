using UnityEngine;

public class Attack : Node
{
    Transform self;
    string targetTag;
    Vector3 range;
    Vector3 originOffset;
    Vector3 direction;
    bool sucess = false;
    public Attack(Transform self, string targetTag, Vector3 range, Vector3 origineOffset, Vector3 direction, Condition[] conditions, BehaviorTree BT) : base(conditions, BT)
    {
        this.self = self;
        this.targetTag = targetTag;
        this.range = range;
        this.originOffset = origineOffset;
        this.direction = direction;
    }

    public override void EvaluateAction()
    {
        base.EvaluateAction();

        RaycastHit[] hits = Physics.BoxCastAll(self.position + originOffset, range / 2, direction);
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.tag == targetTag) // comparer par tag, gameobject ou wtv
                {
                    sucess = true;
                    // target.GetComponent<HealthComponent>().LoseHealth(dmg)
                }
            }
        }
        FinishAction(sucess);
    }

}
