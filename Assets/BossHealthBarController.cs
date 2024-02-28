using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
    #if UNITY_EDITOR
     
    using UnityEditor;
     
    #endif


[RequireComponent(typeof(Damagable))]
public class BossHealthBarController : MonoBehaviour
{

    GameObject boss;

    public Damagable damagable;
    public Image healthBar;
    float currentHealth;
    float maxHealth;
    float healthPercentage;


    

    void Start() {
        boss = GameObject.FindWithTag("Boss");
        damagable = boss.GetComponent<Damagable>();
        maxHealth = damagable.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = damagable.Health;
        healthPercentage = currentHealth / maxHealth;

        healthBar.fillAmount = healthPercentage;

    }
}
