using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    [SerializeField] AimComponent aimComp;
    [SerializeField] float damage = 5f;
    public override void Attack()
    {
        GameObject target = aimComp.GetAimTarget();
        DamageGameObject(target, damage);
    }
}
