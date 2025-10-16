using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState2D : PlayerState2D
{
    private float jumpForce = 50f; // 基础跳跃力
    private float maxJumpHeight = 8f; // 最大跳跃高度
    private float minJumpHeight = 0.5f; // 最小跳跃高度
    private float jumpTimeCounter = 0f; // 跳跃计时器
    private float maxJumpTime = .6f; // 最大跳跃持续时间
    private bool isJumping = false;
    private float startY; // 跳跃起始Y位置

    public JumpState2D(PlayerStateMachine2D _stateMachine, Player2D _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.GetComponent<AdvancedSquashStretch>().isMoving = false;

        // 初始化跳跃
        isJumping = true;
        jumpTimeCounter = maxJumpTime;
        startY = player.transform.position.y;

        // 应用初始跳跃力
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        // 设置动画参数
        player.anim.SetFloat("yDir", 1f); // 初始向上
    }

    public override void Exit()
    {
        base.Exit();
        isJumping = false;
        player.anim.SetFloat("yDir", 0f); // 重置Y方向
    }

    public override void Update()
    {
        base.Update();

        if (isJumping)
        {
            // 检查是否达到最大高度
            float currentHeight = player.transform.position.y - startY;
            if (currentHeight >= maxJumpHeight)
            {
                isJumping = false;
                player.anim.SetFloat("yDir", -1f); // 开始下落
                return;
            }

            // 更新Y方向动画参数
            float yVelocity = rb.velocity.y;
            float normalizedYDir = Mathf.Clamp(yVelocity / jumpForce, -1f, 1f);
            player.anim.SetFloat("yDir", normalizedYDir);

            // 如果按住跳跃键且还有跳跃时间，继续施加跳跃力
            if (Input.GetKey(KeyCode.W) && jumpTimeCounter > 0)
            {
                // 计算递减的跳跃力（按得越久力越小）
                float currentJumpForce = jumpForce * (jumpTimeCounter / maxJumpTime);
                rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                // 松开跳跃键或跳跃时间用完，停止跳跃
                isJumping = false;

                // 如果当前还在上升，设置yDir为0（顶点）
                if (rb.velocity.y > 0)
                    player.anim.SetFloat("yDir", 0f);
                else
                    player.anim.SetFloat("yDir", -1f); // 开始下落
            }

            // 检查是否开始下落
            if (rb.velocity.y <= 0)
            {
                player.anim.SetFloat("yDir", -1f); // 下落中
            }
        }
        else
        {
            // 跳跃结束，更新Y方向
            player.anim.SetFloat("yDir", Mathf.Clamp(rb.velocity.y / jumpForce, -1f, 0f));

            // 检查是否落地
            if (player.IsGroundDetected() && rb.velocity.y <= 0)
            {
                stateMachine.ChangeState(player.idleState);
            }
        }

        // 水平移动
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * player.moveSpeed, rb.velocity.y);
        }
    }
}