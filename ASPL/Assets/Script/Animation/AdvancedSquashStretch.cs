//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AdvancedSquashStretch : MonoBehaviour
//{
//    [Header("形变参数")]
//    public AnimationCurve squashCurve; // 挤压动画曲线
//    public float cycleDuration = 0.5f; // 完整形变周期

//    private Vector3 originalScale;
//    private float timer = 0f;
//    public bool isMoving = false;

//    void Start()
//    {
//        originalScale = transform.localScale;
//    }

//    void Update()
//    {
//        float horizontal = Input.GetAxis("Horizontal");
//        isMoving = Mathf.Abs(horizontal) > 0.1f;

//        if (isMoving)
//        {
//            // 计时器循环
//            timer += Time.deltaTime;
//            if (timer > cycleDuration) timer = 0f;

//            // 使用动画曲线计算形变值
//            float curveValue = squashCurve.Evaluate(timer / cycleDuration);

//            // 应用形变
//            ApplySquashStretch(curveValue);
//        }
//        else
//        {
//            // 停止移动时重置形变
//            timer = 0f;
//            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * 5f);
//        }
//    }

//    void ApplySquashStretch(float value)
//    {
//        // 根据曲线值计算缩放
//        // 假设曲线在0.5处为挤压最低点，0和1处为拉伸最高点
//        float squash = Mathf.Abs(value - 0.5f) * 2f; // 转换为0-1范围

//        Vector3 newScale = new Vector3(
//            originalScale.x * (1 + squash * 0.2f), // X轴拉伸
//            originalScale.y * (1 - squash * 0.3f), // Y轴挤压
//            originalScale.z
//        );

//        transform.localScale = newScale;
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedSquashStretch : MonoBehaviour
{
    [Header("形变参数")]
    public AnimationCurve squashCurve; // 挤压动画曲线
    public float cycleDuration = 0.5f; // 完整形变周期
    [Range(0f, 1f)]
    public float lowestPoint = 0.8f; // 挤压最低点在周期中的位置 (0-1)

    private Vector3 originalScale;
    private float timer = 0f;
    public bool isMoving = false;

    void Start()
    {
        originalScale = transform.localScale;

        // 如果没有设置曲线，创建一个默认曲线
        if (squashCurve == null || squashCurve.keys.Length == 0)
        {
            CreateDefaultCurve();
        }
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
        // 现在最低点由lowestPoint参数控制
        float normalizedTime = timer / cycleDuration;

        // 计算与最低点的距离（考虑周期循环）
        float distanceToLowest = Mathf.Abs(normalizedTime - lowestPoint);
        distanceToLowest = Mathf.Min(distanceToLowest, 1 - distanceToLowest);

        // 将距离转换为挤压强度
        float squash = (1 - distanceToLowest * 2) * value;

        Vector3 newScale = new Vector3(
            originalScale.x * (1 + squash * 0.2f), // X轴拉伸
            originalScale.y * (1 - squash * 0.3f), // Y轴挤压
            originalScale.z
        );

        transform.localScale = newScale;
    }

    void CreateDefaultCurve()
    {
        // 创建默认曲线，最低点在指定位置
        squashCurve = new AnimationCurve();

        // 起点较高
        squashCurve.AddKey(0f, 1.2f);

        // 最低点在指定位置
        squashCurve.AddKey(lowestPoint, 0.6f);

        // 终点较高
        squashCurve.AddKey(1f, 1.2f);

        // 使曲线平滑
        for (int i = 0; i < squashCurve.keys.Length; i++)
        {
            squashCurve.SmoothTangents(i, 0.5f);
        }
    }

    // 在Inspector中修改lowestPoint时自动更新曲线
    private void OnValidate()
    {
        if (squashCurve != null && squashCurve.keys.Length >= 3)
        {
            // 更新曲线的最低点位置
            UpdateCurveLowestPoint();
        }
    }

    void UpdateCurveLowestPoint()
    {
        // 获取当前曲线的关键帧
        Keyframe[] keys = squashCurve.keys;

        // 更新最低点关键帧的位置
        keys[1].time = lowestPoint;

        // 重新设置关键帧
        squashCurve.keys = keys;

        // 平滑曲线
        for (int i = 0; i < squashCurve.keys.Length; i++)
        {
            squashCurve.SmoothTangents(i, 0.5f);
        }
    }
}
