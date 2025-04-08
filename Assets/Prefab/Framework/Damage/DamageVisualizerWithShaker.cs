using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVisualizerWithShaker : DamageVisualiser
{
    [SerializeField] Shaker shaker;

    protected override void ToolDamage(float health, float delta, float maxHealth, GameObject instigator)
    {
        base.ToolDamage(health, delta, maxHealth, instigator);
        if(shaker != null)
        {
            shaker.StartShake();
        }
    }
}
