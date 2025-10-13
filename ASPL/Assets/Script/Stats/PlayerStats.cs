using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    Player player;

    public Stats maxEnergy;
    public int currentEnergy;
    public int getEnergyEveryAttack;
    public int useEnergyOnceNeed;

    public int canReviveCounter = 0;

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
        currentEnergy = 0;
    }

    public override void DoDamage(CharacterStats _targetStats)
    {
        base.DoDamage(_targetStats);
        GetEnergy();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        if (currentHealth > 0)
            player.stateMachine.ChangeState(player.hitState);

        player.damageEffect();
    }

    protected override void Die()
    {
        base.Die();
        player.Die();
    }

    public void GetEnergy()
    {
        if (currentEnergy < GetMaxEnergyValue())
        {
            if (currentEnergy + getEnergyEveryAttack >= GetMaxEnergyValue())
            {
                currentEnergy = GetMaxEnergyValue();
                return;
            }
            currentEnergy += getEnergyEveryAttack;
        }
        return;
    }

    public bool CanUseEnergy()
    {
        if (currentEnergy >= useEnergyOnceNeed)
        {

            return true;
        }
        return false;
    }

    public void UseEnergy()
    {
        if (CanUseEnergy())
        {
            currentEnergy -= useEnergyOnceNeed;
        }
    }

    public int GetMaxEnergyValue()
    {
        return maxEnergy.GetValue();
    }


    public void RevivePlayer()
    {
        if (canReviveCounter <= 0)
            return;
        canReviveCounter--;
        StartHealthRecovery(GetMaxHealthValue());
        player.stateMachine.ChangeState(player.idleState);
    }
}
