using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Ability/Fire")]
public class FireAbility : Ability
{
    [SerializeField] Scanner ScanerPrefab;
    [SerializeField] float fireRadius;
    [SerializeField] float fireDuration;
    [SerializeField] float damageDuration =3f;
    [SerializeField] float fireDamage=20f;

    [SerializeField] GameObject scanVFX;
    [SerializeField] GameObject DamageVFX;
    public override void ActivateAbility()
    {
        if(!CommitAbility()) return;
        Scanner fireScaner = Instantiate(ScanerPrefab, AbilityComp.transform);
        fireScaner.SetScanRange(fireRadius);
        fireScaner.SetScanDuration(fireDuration);
        

        fireScaner.AddChildAttached(Instantiate(scanVFX).transform);
        fireScaner.onScanDetectionUpdated += DetectionUpdate;
        fireScaner.StartScan();
    }

    private void DetectionUpdate(GameObject newDetection)
    {
        ITeamInterface detectedTeamInterface = newDetection.GetComponent<ITeamInterface>();
        if (detectedTeamInterface == null || detectedTeamInterface.GetRelationTowards(AbilityComp.gameObject) != ETeamRelation.Enemy)
        {
            return;            
        }

        HealthComponent enemyHealthComp = newDetection.GetComponent<HealthComponent>();

        if (enemyHealthComp == null)
            return;
        AbilityComp.StartCoroutine(ApplyDamageTo(enemyHealthComp));
    }

    private IEnumerator ApplyDamageTo(HealthComponent enemyHealthComp)
    {
        GameObject damageVFX = Instantiate(DamageVFX, enemyHealthComp.transform);
        float damageRate = fireDamage / damageDuration;
        float startTime = 0;
        while(startTime <damageDuration && enemyHealthComp != null)
        {
            startTime += Time.deltaTime;
            enemyHealthComp.ChangeHealth(-damageRate * Time.deltaTime, AbilityComp.gameObject);
            yield return new WaitForEndOfFrame();
        }

        if(damageVFX != null)
        {
            Destroy(damageVFX);
        }
    }
}
