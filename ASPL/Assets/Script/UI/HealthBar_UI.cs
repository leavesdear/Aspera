using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity;
    private RectTransform myTransform;
    private CharacterStats myStats;
    public Slider slider;

    private void Start()
    {
        myStats = GetComponentInParent<CharacterStats>();
        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();

        entity.onFlipped += FlipUI;
    }

    private void Update()
    {
        UpdateHealthUI();
    }

    private void FlipUI() => myTransform.Rotate(0, 180, 0);

    private void OnDisable() => entity.onFlipped -= FlipUI;

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }
}
