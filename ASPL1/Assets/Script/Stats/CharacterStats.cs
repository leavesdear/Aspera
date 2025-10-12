using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Defensive stats")]
    public Stats maxHealth;
    public Stats armor;
    public Stats evasion;
    public Stats magicResistance;

    [Header("Major stats")]

    public Stats strength;
    public Stats agiliity;
    public Stats intelligence;
    public Stats vitality;

    [Header("Offensive stats")]
    public Stats critChance;
    public Stats critPower;
    public Stats damage;


    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightningDamage;


    public int currentHealth;
    public float recoverSpeed;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (!_targetStats.GetComponent<Entity>().canBeHurt)//目标在无敌状态下无法被攻击
            return;

        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totleDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totleDamage = CalculateCriticalDamge(totleDamage);
            Debug.Log("CRIT!");
        }

        totleDamage = CheckTargetArmor(totleDamage);

        _targetStats.TakeDamage(totleDamage);

        DoMagicDamage(_targetStats);
    }

    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetRestistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
            return;

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {

            if (Random.value < .3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("FIRE!");
                return;
            }

            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("ICE!");
            }
            if (Random.value < .5f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Lightning!");
                return;
            }

        }


        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);

    }

    private static int CheckTargetRestistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (_chill || _shock || _ignite)//在有异常状态的情况下不在挂异常
            return;
        isChilled = _chill;
        isShocked = _shock;
        isIgnited = _ignite;
    }


    private int CheckTargetArmor(int totleDamage)
    {
        totleDamage -= armor.GetValue();
        totleDamage = Mathf.Clamp(totleDamage, 0, int.MaxValue);
        return totleDamage;
    }

    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agiliity.GetValue();
        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("ATTACK AVOIDED");
            return true;
        }
        return false;
    }

    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agiliity.GetValue();
        if (Random.Range(0, 100) < totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        if (currentHealth < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {

    }

    private int CalculateCriticalDamge(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;

        float critDamge = _damage + totalCritPower;

        return Mathf.RoundToInt(critDamge);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    #region RecoverHealth

    protected Coroutine recoveryCoroutine; // 用于存储协程引用

    // 开始恢复生命值
    public void StartHealthRecovery(int targetHealth)
    {
        // 如果已有恢复协程在运行，先停止
        if (recoveryCoroutine != null)
        {
            StopCoroutine(recoveryCoroutine);
        }
        recoveryCoroutine = StartCoroutine(RecoverHealth(targetHealth));
    }

    // 恢复生命值的协程
    protected IEnumerator RecoverHealth(int targetHealth)
    {
        float startHealth = currentHealth;
        float difference = targetHealth - startHealth;

        // 如果目标值小于等于当前值，直接设置并退出
        if (difference <= 0)
        {
            currentHealth = targetHealth;
            yield break;
        }

        float duration = difference / recoverSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentHealth = (int)Mathf.Lerp(startHealth, targetHealth, elapsed / duration);
            yield return null;
        }

        currentHealth = targetHealth; // 确保最终值准确
    }
    #endregion
}
