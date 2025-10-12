using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    //Enemy_Boss enemy;
    Enemy enemy;
    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy>(); //enemy = GetComponent<Enemy_Boss>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        //enemy.beAttacked = true;

        enemy.damageEffect();
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
        //enemy.stateMechine.changeState(enemy.deadState);
    }
}
