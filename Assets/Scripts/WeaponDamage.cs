using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] PlayerStats stats;
    float damageGiven;

    void Start()
    {
        damageGiven = stats.damage;
    }

    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (CompareTag("Club")) {
            if (collider.gameObject.CompareTag("Character"))
            {
                collider.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageGiven);
            }
        }
    }
}
