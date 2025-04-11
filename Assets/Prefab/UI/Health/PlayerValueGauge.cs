using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerValueGauge : MonoBehaviour
{
    [SerializeField] Image amtImage;
    [SerializeField] TextMeshProUGUI amtText;

    internal void UpdateValue(float health, float delta, float maxHealth)
    {
        amtImage.fillAmount = health/maxHealth;
        int healthAsInt = (int)health;
        amtText.SetText(healthAsInt.ToString());
    }
}
