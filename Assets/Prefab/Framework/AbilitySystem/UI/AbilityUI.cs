using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    Ability ability;
    [SerializeField] Image AbilityIcon;
    [SerializeField] Image cooldownWheel;

    [SerializeField] float hightlightSize = 1.5f;
    [SerializeField] float hightOffset = 200f;
    [SerializeField] float scaleSpeed = 20f;
    [SerializeField] RectTransform offsetPivot;

    Vector3 goalScale = Vector3.one;
    Vector3 goalOffset = Vector3.zero;


    bool bIsOnCooldown = false;
    float CooldownCounter = 0f;

    public void SetScaleAmt(float amt)
    {
        goalScale = Vector3.one * (1 + (hightlightSize - 1) * amt);
        goalOffset = Vector3.left * hightOffset * amt;
    }

    public void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, goalScale, Time.deltaTime * scaleSpeed);
        offsetPivot.localPosition = Vector3.Lerp(offsetPivot.localPosition, goalOffset, Time.deltaTime * scaleSpeed);
    }
    internal void Init(Ability newAbility)
    {
        ability= newAbility;
        AbilityIcon.sprite = newAbility.GetAbillityIcon();
        cooldownWheel.enabled = false;
        ability.onCooldownStarted += StartCooldown;
    }

    private void StartCooldown()
    {
        if (bIsOnCooldown)
            return;

        StartCoroutine(CooldownCoroutine());
    }
    internal void ActivateAbility()
    {
        ability.ActivateAbility();
    }
    IEnumerator CooldownCoroutine()
    {
        bIsOnCooldown= true;
        CooldownCounter = ability.GetCooldownDuration();

        float cooldownDuration = CooldownCounter;
        cooldownWheel.enabled = true;

        while (CooldownCounter > 0)
        {
            CooldownCounter -=Time.deltaTime;
            cooldownWheel.fillAmount = CooldownCounter / cooldownDuration;
            yield return new WaitForEndOfFrame();
        }
        bIsOnCooldown = false;
        cooldownWheel.enabled = false;
    }
}
