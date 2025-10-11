using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimatorTrigger : MonoBehaviour
{
    private Enemy_Boss enemy => GetComponentInParent<Enemy_Boss>();
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
                AudioManager.instance.PlaySFX(9);
            }
        }
    }

    private void AttackMoveToPlayer()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(enemy.attackCheck.position, 100);

        foreach (var hit in collider)
        {
            if (hit.GetComponent<Player>() != null)
            {
                Player target = hit.GetComponent<Player>();
                Vector3 dir = (target.transform.position - enemy.transform.position).normalized;
                enemy.transform.position += enemy.moveToPlayerDiatance * dir;
                enemy.FlipToPlayerWhileAttack();
            }
        }
    }

    private IEnumerator CameraShakeTrigger()
    {
        AudioManager.instance.PlaySFX(4);
        Camera.main.transform.position += new Vector3(0, -1, 1) * 2;
        yield return new WaitForSeconds(0.02f);
        Camera.main.transform.position += new Vector3(0, 1, -1) * 4;
        yield return new WaitForSeconds(0.02f);
        Camera.main.transform.position += new Vector3(0, -1, 1) * 2;
    }
}
