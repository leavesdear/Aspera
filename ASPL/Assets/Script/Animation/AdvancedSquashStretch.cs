using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedSquashStretch : MonoBehaviour
{
    [Header("形变参数")]
    public AnimationCurve squashCurve; // 挤压动画曲线
    public float cycleDuration = 0.5f; // 完整形变周期

    private Vector3 originalScale;
    private float timer = 0f;
    public bool isMoving = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        isMoving = Mathf.Abs(horizontal) > 0.1f;

        if (isMoving)
        {
            // 计时器循环
            timer += Time.deltaTime;
            if (timer > cycleDuration) timer = 0f;

            // 使用动画曲线计算形变值
            float curveValue = squashCurve.Evaluate(timer / cycleDuration);

            // 应用形变
            ApplySquashStretch(curveValue);
        }
        else
        {
            // 停止移动时重置形变
            timer = 0f;
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * 5f);
        }
    }

    void ApplySquashStretch(float value)
    {
        // 根据曲线值计算缩放
        // 假设曲线在0.5处为挤压最低点，0和1处为拉伸最高点
        float squash = Mathf.Abs(value - 0.5f) * 2f; // 转换为0-1范围

        Vector3 newScale = new Vector3(
            originalScale.x * (1 + squash * 0.2f), // X轴拉伸
            originalScale.y * (1 - squash * 0.3f), // Y轴挤压
            originalScale.z
        );

        transform.localScale = newScale;
    }
}
