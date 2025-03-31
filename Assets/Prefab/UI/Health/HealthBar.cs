using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    private Transform attachPoint;

    public void Init(Transform _attachPoint)
    {
        attachPoint = _attachPoint;
    }
    public void SetHeathSliderValue(float health, float delta, float maxHealth)
    {
        healthSlider.value = health/maxHealth;

    }
    private void Update()
    {
        Vector3 attachScreenPoint =  Camera.main.WorldToScreenPoint(attachPoint.position);
        transform.position = attachScreenPoint;
    }
    internal void OnOwnerDead()
    {
        Destroy(gameObject);
    }
}
