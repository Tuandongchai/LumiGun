using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] string attachSlotTag;
    [SerializeField] float attackRateMult = 1f;
    [SerializeField] AnimatorOverrideController overrideController;

    public abstract void Attack();

    public string GetAttachSlotTag()
    {
        return attachSlotTag;
    }
    public GameObject owner { get; private set; }

    public void Init(GameObject _owner)
    {
        owner = _owner;
        UnEquip();
    }
    public void Equip()
    {
        gameObject.SetActive(true);
        owner.GetComponent<Animator>().runtimeAnimatorController = overrideController;
        owner.GetComponent<Animator>().SetFloat("attackRateMult", attackRateMult);
    }
    public void UnEquip()
    {
        gameObject.SetActive(false);
    }
    public void DamageGameObject(GameObject objToDamage, float amt)
    {
        HealthComponent healthComp = objToDamage.GetComponent<HealthComponent>();
        healthComp?.ChangeHealth(-amt);
    }
}
