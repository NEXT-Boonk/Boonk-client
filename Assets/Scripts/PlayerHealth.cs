using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] PlayerStats stats;

    float maxHealth;
    [SerializeField] private float currentHealth;
    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = stats.health;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        float fillAmount = currentHealth / maxHealth;
        healthBar.value = fillAmount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if(currentHealth <= 0)
        {
            // Player dead / Should respawn
        }

    }

    public void TakeDamage(float damage)
    {
        currentHealth += damage / (0 - stats.defense/10);
    }


}
