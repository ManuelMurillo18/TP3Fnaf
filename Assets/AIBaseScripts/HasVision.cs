using UnityEngine;

public class HasVision : Condition
{
    GameObject target;
    Transform self;
    float angleView;

    public HasVision(Transform self, GameObject target, float angleView, bool reverseCondition = false)
    {
        this.self = self;
        this.target = target;
        this.angleView = angleView;
        this.reverseCondition = reverseCondition;
    }
    public override bool Evaluate()
    {
        Vector3 directionToTarget = (target.transform.position - self.position).normalized;

        float angleToTarget = Vector3.Angle(self.transform.forward, directionToTarget);

        if (angleToTarget > angleView)
        {
            //Debug.Log("Doesn't see player");
            return CheckForReverseCondition(false);
        }
            
        if (Physics.Raycast(self.position, directionToTarget, out RaycastHit hit))
        {
            if (hit.collider.gameObject != target)
            {
                //Debug.Log("Doesn't see player");
                return CheckForReverseCondition(false);
            }
        }

        //Debug.Log("Has Vision of " + target.name);
        return CheckForReverseCondition(true);
    }
}
