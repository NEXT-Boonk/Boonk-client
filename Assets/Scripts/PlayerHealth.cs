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

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) return;
        maxHealth = stats.health;
        currentHealth = maxHealth;
        

    }

    // Update is called once per frame
    void Update()
    {     
        healthBar.gameObject.SetActive(false);
        if(!IsOwner) return; 
        healthBar.gameObject.SetActive(true);  
        float fillAmount = currentHealth / maxHealth;
        healthBar.value = fillAmount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if(currentHealth <= 0)
        {
            this.GetComponent<PlayerNetwork>().playerSpawn();         
            currentHealth = maxHealth;
        }

    }

    public void TakeDamage(float damage)
    {
        if (!IsOwner) return;
        currentHealth += damage / (0 - stats.defense/10);
    }


}
