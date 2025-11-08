using UnityEngine;

public class MonsterBullet : MonoBehaviour
{

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit by monster bullet");
            ManuPlayerComp playerComp = other.gameObject.GetComponent<ManuPlayerComp>();
            if (playerComp != null)
                playerComp.TakeDamage(Random.Range(1, 15));
            
           Destroy(this.gameObject);
        }
        
    }
}
