using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    float maxHealth;
    public float currentHealth;
    public Slider healthBar;

    [SerializeField] PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = stats.health;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
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
        currentHealth -= damage / (1 - stats.defense);
    }


}
