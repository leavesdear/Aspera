using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CloneClash_Skill : Skill
{
    public float arcHeight = 3f;
    public float arcDuration = 1.5f;
    public float horizontalSpeed = 5f;
    public float xOffset = 3f;


    private float horizontalDuration;

    private Vector3 targetPosition;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        horizontalDuration = xOffset / horizontalSpeed * 2;
        targetPosition = player.transform.position + new Vector3(-xOffset, 0, 0);
        StartCoroutine("AttackRoutine");
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    IEnumerator AttackRoutine()
    {
        boss.anim.SetInteger("clone", 1);
        // 阶段一：弧形移动
        Vector3 startPos = boss.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < arcDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / arcDuration;

            // 抛物线运动公式
            float yOffset = Mathf.Sin(progress * Mathf.PI) * arcHeight;
            boss.transform.position = Vector3.Lerp(startPos, targetPosition, progress)
                                + new Vector3(0, yOffset, 0);

            yield return null;
        }
        boss.anim.SetInteger("clone", 2);
        // 阶段二：水平滑行
        Vector3 moveDirection = Vector3.right; // 向右移动
        elapsedTime = 0;
        while (elapsedTime < horizontalDuration)
        {
            elapsedTime += Time.deltaTime;
            boss.transform.Translate(moveDirection * horizontalSpeed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(RecoveryTime());

    }

    IEnumerator RecoveryTime()
    {
        boss.anim.SetInteger("clone", 3);
        yield return new WaitForSeconds(recoveryTime);
        boss.stateMachine.ChangeState(boss.battleState);
    }
}
