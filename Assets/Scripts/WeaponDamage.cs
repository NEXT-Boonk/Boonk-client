using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] PlayerStats stats;
    float damageGiven;

    void Start()
    {
        //damageGiven = stats.Damage;
        damageGiven = stats.damage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (this.CompareTag("Club")) { 
            if (col.gameObject.CompareTag("Character"))
            {
                col.gameObject.GetComponent<HealthSystem>().TakeDamage(damageGiven);
            }
        }
    }
}
