using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
//    [SerializeField] PlayerStats stats;

    float damageGiven;

    void Start()
    {
        //damageGiven = stats.damage;
        damageGiven = 30;
    }

    

    private void OnTriggerEnter(Collider collider)
    {
        if (CompareTag("Club")) { 
            if (collider.gameObject.CompareTag("Character"))
            {
                collider.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageGiven);
            }
        }

        if (CompareTag("Rock")) { 
            if (collider.gameObject.CompareTag("Character"))
            {
                collider.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageGiven);
            }
        }

        if (CompareTag("Arrow")) { 
            if (collider.gameObject.CompareTag("Character"))
            {
                collider.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageGiven);
            }
        }
    }
}
