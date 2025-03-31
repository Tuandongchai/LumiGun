using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField] AimComponent aimComp;
    [SerializeField] float damage = 20f;
    public override void Attack()
    {
        GameObject target = aimComp.GetAimTarget();
        DamageGameObject(target, damage);
    }
}
