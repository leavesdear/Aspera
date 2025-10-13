using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlosStats : CharacterStats
{
    Enemy_Flos enemy;
    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy_Flos>();
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        if (currentHealth > 0)
            enemy.stateMachine.ChangeState(enemy.hitState);

        enemy.damageEffect();
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
        //enemy.stateMechine.changeState(enemy.deadState);
    }
}