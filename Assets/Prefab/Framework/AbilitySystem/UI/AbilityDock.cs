using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityDock : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] AbilityComponent abilityComponent;
    [SerializeField] RectTransform root;
    [SerializeField] VerticalLayoutGroup layoutGrp;
    [SerializeField] AbilityUI abilityUIPrefab;

    [SerializeField] float ScaleRange = 200f;

    [SerializeField] float hightlightSize = 1.5f;
    [SerializeField] float scaleSpeed = 20f;

    Vector3 goalScale = Vector3.one;

    List<AbilityUI> abilitiUIs = new List<AbilityUI>();

    PointerEventData touchData;
    AbilityUI hightlightedAbility;
    private void Awake()
    {
        abilityComponent.onNewAbilityAdded += AddAbility;
    }
    private void Update()
    {
        if (touchData != null)
        {
            GetUIUnderPointer(touchData, out hightlightedAbility);
            ArrangeScale(touchData);
        }

        transform.localScale = Vector3.Lerp(transform.localScale, goalScale, Time.deltaTime * scaleSpeed);
    }

    private void ArrangeScale(PointerEventData touchData)
    {
        if (ScaleRange == 0) return;
        float touchVerticalPos = touchData.position.y;
        foreach(AbilityUI abilityUI in abilitiUIs)
        {
            float abilityUIVerticalPos = abilityUI.transform.position.y;
            float distance = Mathf.Abs(touchVerticalPos - abilityUIVerticalPos);

            if (distance > ScaleRange)
            {
                abilityUI.SetScaleAmt(0);
                continue;
            }
            float scaleAmt = (ScaleRange - distance) / ScaleRange;
            abilityUI.SetScaleAmt(scaleAmt);
        }
    }

    private void AddAbility(Ability newAbility)
    {
        AbilityUI newAbilityUI = Instantiate(abilityUIPrefab, root);
        newAbilityUI.Init(newAbility);
        abilitiUIs.Add(newAbilityUI);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        touchData = eventData;
        goalScale = Vector3.one * hightlightSize;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (hightlightedAbility)
            hightlightedAbility.ActivateAbility();

        touchData = null;
        ResetScale();
        goalScale = Vector3.one ;
    }

    private void ResetScale()
    {
        foreach(AbilityUI abilityUI in abilitiUIs)
        {
            abilityUI.SetScaleAmt(0);
        }
    }

    private bool GetUIUnderPointer(PointerEventData eventData, out AbilityUI abilityUI)
    {
        List<RaycastResult> findAbility = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, findAbility);

        abilityUI = null;
        foreach(RaycastResult result in findAbility)
        {
            abilityUI = result.gameObject.GetComponentInParent<AbilityUI>();
            if(abilityUI != null)
            {
                return true;
            }
        }
        return false;
    }
}
