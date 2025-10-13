using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        //Collider[] collider = Physics.OverlapSphere(player.attackCheck.position, player.attackCheckRadius, player.enemyLayer, QueryTriggerInteraction.Collide);
        Collider2D[] collider = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius, player.enemyLayer);
        foreach (var hit in collider)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                player.GetComponent<PlayerStats>().DoDamage(_target);
                AudioManager.instance.PlaySFX(9);
            }
        }
    }

}
