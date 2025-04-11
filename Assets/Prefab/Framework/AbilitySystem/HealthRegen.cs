using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Ability/HealthRegen")]
public class HealthRegen : Ability
{
    [SerializeField] float healthRegenAmt;
    [SerializeField] float healthRegenDuration;

    public override void ActivateAbility()
    {
        if(!CommitAbility()) return;

        HealthComponent HealthComp = AbilityComp.GetComponent<HealthComponent>();
        if(HealthComp != null)
        {
            if(healthRegenDuration == 0)
            {
                HealthComp.ChangeHealth(healthRegenAmt, AbilityComp.gameObject);
            }
            AbilityComp.StartCoroutine(StartHealthRegen(healthRegenAmt, healthRegenDuration, HealthComp));
        }
    }
    private IEnumerator StartHealthRegen(float amt, float duration, HealthComponent healthComp)
    {
        float counter = duration;
        float regenRate = amt / duration;
        while(counter >0)
        {
            counter-=Time.deltaTime;
            healthComp.ChangeHealth(regenRate *Time.deltaTime, AbilityComp.gameObject);
            yield return new WaitForEndOfFrame();
        }
    }
}
