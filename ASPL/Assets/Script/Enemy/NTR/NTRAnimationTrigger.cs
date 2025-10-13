using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTRAnimationTrigger : MonoBehaviour
{
    private Enemy_NTR enemy => GetComponentInParent<Enemy_NTR>();
    public GameObject effetPrefab;

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void ExpolsionTrigger()
    {
        GameObject explosion = Instantiate(effetPrefab, enemy.attackCheck.transform.position, Quaternion.identity);
        explosion.transform.localScale = Vector3.one * 6;//爆炸效果太小所以放大
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

    private IEnumerator CameraShakeTrigger()
    {
        AudioManager.instance.PlaySFX(9);
        Camera.main.transform.position += new Vector3(0, -1, 1) * 2;
        yield return new WaitForSeconds(0.02f);
        Camera.main.transform.position += new Vector3(0, 1, -1) * 4;
        yield return new WaitForSeconds(0.02f);
        Camera.main.transform.position += new Vector3(0, -1, 1) * 2;
    }
    private void AttackWindowOpen() => enemy.AttackWindowOpen();
    private void AttackWindowClose() => enemy.AttackWindowClose();
}
