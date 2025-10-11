using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("参数")]
    public float shakeDuration = 0.5f;
    public float maxIntensity = 0.1f;
    public AnimationCurve decayCurve; // 在Inspector中设置曲线（如从1到0的衰减）

    private Vector3 originalPos;
    private bool isShaking = false;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    // 通过协程实现精准时间控制
    public void StartShake()
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeCoroutine());
        }
    }

    private IEnumerator ShakeCoroutine()
    {
        isShaking = true;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            // 通过曲线控制衰减
            float strength = decayCurve.Evaluate(elapsed / shakeDuration) * maxIntensity;

            // 使用Perlin噪声生成平滑随机值
            float x = Mathf.PerlinNoise(Time.time * 10, 0) * 2 - 1;
            float y = Mathf.PerlinNoise(0, Time.time * 10) * 2 - 1;

            transform.localPosition = originalPos + new Vector3(0, x, y) * strength;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        isShaking = false;
    }
}
