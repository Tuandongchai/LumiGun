using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] Image amtImage;
    [SerializeField] TextMeshProUGUI amtText;

    internal void UpdateHealth(float health, float delta, float maxHealth)
    {
        amtImage.fillAmount = health/maxHealth;
        amtText.SetText(health.ToString());
    }
}
