using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] CanvasGroup GameplayControl;
    [SerializeField] CanvasGroup GameplayMenu;

    public void SetGameplayControlEnabled(bool enabled)
    {
        SetCanvasGroupEnabled(GameplayControl, enabled);
    }

    private void SetGameplayMenuEnabled(CanvasGroup gameplayControl, bool enabled)
    {
        SetCanvasGroupEnabled(GameplayMenu, enabled);
    }
    private void SetCanvasGroupEnabled(CanvasGroup grp, bool enabled)
    {
        grp.interactable = enabled;
        grp.blocksRaycasts = enabled;
    }
}
