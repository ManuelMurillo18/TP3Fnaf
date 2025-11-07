using UnityEngine;

public class AttackCooldown : MonoBehaviour
{
    [SerializeField] private float cooldown = 20f;
    private float timer = 0f;
    public bool canAttack = false;

    void Update()
    {
        
        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            canAttack = true;
            timer = 0f; 
        }
    }

    
    public void Attacked()
    {
        canAttack = false;
        timer = 0f;
    }
}

    

