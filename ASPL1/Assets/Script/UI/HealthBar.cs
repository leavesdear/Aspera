using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthPointImage;
    public Image healthPointEffect;
    public float effectSpeed = 0.03f; // 暴露给Inspector调整

    public CharacterStats myStats;

    private void Start()
    {

    }

    private void Update()
    {
        healthPointImage.fillAmount = (float)myStats.currentHealth / (float)myStats.GetMaxHealthValue();

        if (healthPointEffect.fillAmount > healthPointImage.fillAmount)
        {
            healthPointEffect.fillAmount -= effectSpeed;
        }
        else
        {
            healthPointEffect.fillAmount = healthPointImage.fillAmount;
        }

    }
}
