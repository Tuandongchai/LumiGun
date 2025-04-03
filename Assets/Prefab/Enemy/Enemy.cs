using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] Animator animator;
    [SerializeField] PerceptionComponent perceptionComp;

    GameObject target;
    private void OnEnable()
    {
        perceptionComp.onPerceptionTargetChanged += TargetChanged;
        if (healthComponent == null)
            return;
        healthComponent.onHealthEmpty += StartDeath;
        healthComponent.onTakeDamage += TakenDamage;

    }
    private void OnDisable()
    {
        healthComponent.onHealthEmpty -= StartDeath;
        healthComponent.onTakeDamage -= TakenDamage;
        perceptionComp.onPerceptionTargetChanged -= TargetChanged;
        
    }
    private void TargetChanged(GameObject _target, bool sensed)
    {
        if(sensed)
        {
            this.target = _target;
        }
        else
        {
            this.target = null;
        }
    }
    private void TakenDamage(float health, float delta, float maxHealth, GameObject instigagor)
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

    private void OnDrawGizmos()
    {
        if(target != null)
        {
            Vector3 drawTargetPos = target.transform.position + Vector3.up;
            Gizmos.DrawWireSphere(drawTargetPos, 0.7f);

            Gizmos.DrawLine(transform.position + Vector3.up, drawTargetPos);
        }
    }
}
