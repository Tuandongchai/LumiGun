using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamageComponent : DamageComponent
{
    [SerializeField] float damage;
    [SerializeField] BoxCollider trigger;
    [SerializeField] bool startedEnabled = false;

    public void SetDamageEnabled(bool enabled)
    {
        trigger.enabled = enabled;

    }
    private void Start()
    {
        SetDamageEnabled(startedEnabled);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!ShouldDamage(other.gameObject))
            return;
        HealthComponent healthComp = other.GetComponent<HealthComponent>();
        if(healthComp != null)
        {
            healthComp.ChangeHealth(-damage, gameObject);
        }
    }
}
