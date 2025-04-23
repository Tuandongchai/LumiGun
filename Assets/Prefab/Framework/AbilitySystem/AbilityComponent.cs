using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityComponent : MonoBehaviour, IPurchaseListener, IRewardListener
{
    [SerializeField] Ability[] initialAbilities;

    public delegate void OnNewAbilityAdded(Ability newAbility);
    public delegate void OnStaminaChange(float newAmount, float maxAmout);


    public event OnNewAbilityAdded onNewAbilityAdded;
    public event OnStaminaChange onStaminaChange;

    private List<Ability> abilities= new List<Ability>();

    [SerializeField] float stamina = 200f;
    [SerializeField] float maxStamina = 200f;

    public void BroadcastStaminaChangeImmediately()
    {
        onStaminaChange?.Invoke(stamina, maxStamina);
    }


    private void Start()
    {
        foreach (Ability ability in initialAbilities)
        {
            GiveAbility(ability);
        }
    }
    void GiveAbility(Ability ability)
    {
        Ability newAbility = Instantiate(ability);
        newAbility.InitAbility(this);
        abilities.Add(newAbility);
        onNewAbilityAdded?.Invoke(newAbility);
    }
    public void ActivateAbility(Ability abilityToActivate)
    {
        if (abilities.Contains(abilityToActivate))
        {
            abilityToActivate.ActivateAbility();
        }     
    }
    float GetStamina()
    {
        return stamina;
    }
    public bool TryConsumeStamina(float staminaToConsume)
    {
        if (stamina <= staminaToConsume) return false;
        stamina -= staminaToConsume;
        BroadcastStaminaChangeImmediately();
        return true;
    }

    public bool HandlePurchase(Object newPurchase)
    {
        Ability itemAsAbility = newPurchase as Ability;

        if(itemAsAbility == null) return false;

        GiveAbility(itemAsAbility);

        return true;
    }

    public void Reward(Reward reward)
    {
        stamina = Mathf.Clamp(stamina + reward.staminaReward, 0, maxStamina);
        BroadcastStaminaChangeImmediately();
    }
}
