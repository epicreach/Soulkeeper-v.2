using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarController : MonoBehaviour
{
   GameObject player;

    public Damagable damagable;
    public Image healthBar;
    public Image healthBorder;
    float currentHealth;
    float maxHealth;
    float healthPercentage;


    

    void Start() {
        player = GameObject.FindWithTag("Player");
        damagable = player.GetComponent<Damagable>();
        maxHealth = damagable.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = damagable.Health;
        healthPercentage = currentHealth / maxHealth;

        healthBar.fillAmount = healthPercentage;
        healthBorder.fillAmount = healthPercentage;

    }
}
