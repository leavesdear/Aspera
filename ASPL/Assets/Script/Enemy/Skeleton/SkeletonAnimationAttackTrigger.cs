using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlosAnimationAttackTrigger : MonoBehaviour
{
    private Enemy_Flos enemy => GetComponent<Enemy_Flos>();
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }


    private void AttackTrigger()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in collider)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.GetComponent<EnemyStats>().DoDamage(target);
            }
        }
    }

    private void AttackWindowOpen() => enemy.AttackWindowOpen();
    private void AttackWindowClose() => enemy.AttackWindowClose();
}
