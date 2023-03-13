using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    // Start is called before the first frame update

    float damageGiven;

    void Start()
    {
        //damageGiven = stats.Damage;
        damageGiven = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("collision");
        if (col.gameObject.CompareTag("Character"))
        {
            col.gameObject.GetComponent<HealthSystem>().TakeDamage(damageGiven);
            Debug.Log("collision with character");
        }
    }
}
