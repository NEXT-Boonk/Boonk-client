using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    float maxHealth;
    public float currentHealth;
    public Slider healthBar;
    public GameObject deathScreen;
    float fillAmount;

    [SerializeField] PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = stats.health;
        currentHealth = maxHealth;
        deathScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        fillAmount = currentHealth / maxHealth;
        Debug.Log(fillAmount);
        healthBar.value = fillAmount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if(currentHealth <= 0)
        {
            deathScreen.SetActive(true);
        }

    }

    public void TakeDamage(float d)
    {

        Debug.Log("this gives damage");
        currentHealth -= d/(1-stats.defense);
        Debug.Log(d);
    }


}
