using UnityEngine;
//Pour que tu comprenne mieux le fonctionnement 
public class AttackCooldownCondition : Condition
{
    private readonly float cooldownTime;
    private readonly float triggerDuration = 5f;  
    private float nextReadyTime;
    private float activationEndTime;
    private bool inActivationLapse; // Lapse de temps ou on peux active le ultimatate
    //On utilise time.time parce que on ne peut pas utilisé delta.time (on ne update pas ceci) c'est le interrupt qui nous appel 

    public AttackCooldownCondition(float cooldownTime, bool reverseCondition = false)
    {
        this.cooldownTime = cooldownTime;
        this.reverseCondition = reverseCondition;
        this.nextReadyTime = Time.time + cooldownTime;  
        this.inActivationLapse = false;

        Debug.Log($"Cooldown started at {Time.time}");
    }

    public override bool Evaluate()
    {
        float currentTime = Time.time;

        // si on est dans le lapse de temps ou l'on peut acitver l'attaque ultime
        // à première vue ce if sert a rien parce que tu reverse la condition en bas mais a cause que le interrupt est à 100ms si on enlèverais
        //ce if (et l'autre d'après) notre interrupt roule trop vite et pourrait rappeler cette condition et BOUM on revient a false à l,écart de 100ms
        // ce qui donne pas le temps à nôtre attack ultime de déclencher 
        if (inActivationLapse)
        {
            if (currentTime <= activationEndTime)
            {
                //Si on vérifie la condition (grâce au interrupt) et on constate qu'on est dans le lapse de temps pour partir l'attaque ultime 
                return CheckForReverseCondition(true);
            }
            else
            {
                // Le lapse de temps d'activation est expiré donc on repart le cooldown  
                inActivationLapse = false;
                nextReadyTime = currentTime + cooldownTime;
            }
        }

        // Cooldown expiré donc on rentre dans le lapse de temps ou l'on peut activer l'ultime
        if (currentTime >= nextReadyTime && !inActivationLapse)
        {
            inActivationLapse = true;
            activationEndTime = currentTime + triggerDuration;
            return CheckForReverseCondition(true);
        }

        Debug.Log($"En attente {nextReadyTime - currentTime:F2} restante");
        return CheckForReverseCondition(false);
    }
}
