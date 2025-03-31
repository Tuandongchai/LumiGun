using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] Animator animator;

    private void OnEnable()
    {
        if (healthComponent == null)
            return;
        healthComponent.onHealthEmpty += StartDeath;
        healthComponent.onTakeDamage += TakenDamage;

    }
    private void OnDisable()
    {
        healthComponent.onHealthEmpty -= StartDeath;
        healthComponent.onTakeDamage -= TakenDamage;
        
    }

    private void TakenDamage(float health, float delta, float maxHealth)
    {

    }

    private void StartDeath()
    {
        TriggerDeathAnimation();
    }
    private void TriggerDeathAnimation()
    {
        if (animator == null)
            return;
        animator.SetTrigger("Dead");
    }
    public void OnDeathAnimationFinished()
    {
        Destroy(gameObject);
    }
}
