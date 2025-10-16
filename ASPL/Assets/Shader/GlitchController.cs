using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour
{
    [Header("基础设置")]
    [Range(0, 1)] public float baseIntensity = 0f;
    [Range(0, 1)] public float maxIntensity = 0.8f;

    [Header("自动故障")]
    public bool enableAutoGlitch = true;
    public float minInterval = 1f;
    public float maxInterval = 4f;
    public float heavyGlitchChance = 0.3f;

    [Header("效果强度")]
    [Range(0, 1)] public float noiseIntensity = 0.5f;
    [Range(0, 1)] public float stripesIntensity = 0.5f;
    [Range(0, 1)] public float distortionIntensity = 0.3f;

    private SpriteRenderer spriteRenderer;
    private Material glitchMaterial;
    private float nextGlitchTime;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            glitchMaterial = new Material(spriteRenderer.material);
            spriteRenderer.material = glitchMaterial;
        }

        nextGlitchTime = Time.time + Random.Range(minInterval, maxInterval);
        UpdateAllParameters();
    }

    void Update()
    {
        if (glitchMaterial == null) return;

        // 更新基础强度
        glitchMaterial.SetFloat("_GlitchIntensity", baseIntensity);

        // 自动随机故障
        if (enableAutoGlitch && Time.time >= nextGlitchTime)
        {
            if (Random.value < heavyGlitchChance)
            {
                TriggerHeavyGlitch();
            }
            else
            {
                TriggerLightGlitch();
            }
        }
    }

    void TriggerLightGlitch()
    {
        float duration = Random.Range(0.1f, 0.3f);
        float intensity = Random.Range(0.2f, 0.5f);
        StartCoroutine(GlitchRoutine(intensity, duration));
        nextGlitchTime = Time.time + Random.Range(minInterval, maxInterval);
    }

    void TriggerHeavyGlitch()
    {
        float duration = Random.Range(0.3f, 0.8f);
        float intensity = Random.Range(0.6f, maxIntensity);
        StartCoroutine(GlitchRoutine(intensity, duration));
        nextGlitchTime = Time.time + Random.Range(minInterval * 0.5f, maxInterval * 0.5f);
    }

    System.Collections.IEnumerator GlitchRoutine(float targetIntensity, float duration)
    {
        float timer = 0f;
        float startIntensity = baseIntensity;

        while (timer < duration)
        {
            float t = timer / duration;
            // 使用曲线让故障效果更自然
            float currentIntensity = Mathf.Lerp(startIntensity, targetIntensity, t);

            // 添加一些随机波动
            float fluctuation = Mathf.Sin(t * Mathf.PI * 8) * 0.1f * targetIntensity;
            currentIntensity += fluctuation;

            glitchMaterial.SetFloat("_GlitchIntensity", currentIntensity);

            timer += Time.deltaTime;
            yield return null;
        }

        glitchMaterial.SetFloat("_GlitchIntensity", baseIntensity);
    }

    void UpdateAllParameters()
    {
        if (glitchMaterial != null)
        {
            glitchMaterial.SetFloat("_NoiseIntensity", noiseIntensity);
            glitchMaterial.SetFloat("_StripesIntensity", stripesIntensity);
            glitchMaterial.SetFloat("_Distortion", distortionIntensity);
            glitchMaterial.SetFloat("_GlitchIntensity", baseIntensity);
        }
    }

    // 公共方法
    public void SetBaseIntensity(float intensity)
    {
        baseIntensity = Mathf.Clamp01(intensity);
        UpdateAllParameters();
    }

    public void SetNoiseIntensity(float intensity)
    {
        noiseIntensity = Mathf.Clamp01(intensity);
        glitchMaterial.SetFloat("_NoiseIntensity", noiseIntensity);
    }

    public void SetStripesIntensity(float intensity)
    {
        stripesIntensity = Mathf.Clamp01(intensity);
        glitchMaterial.SetFloat("_StripesIntensity", stripesIntensity);
    }

    public void TriggerCustomGlitch(float intensity, float duration, float noiseAmount = -1, float stripesAmount = -1)
    {
        if (noiseAmount >= 0) SetNoiseIntensity(noiseAmount);
        if (stripesAmount >= 0) SetStripesIntensity(stripesAmount);

        StartCoroutine(GlitchRoutine(intensity, duration));
    }

    void OnDestroy()
    {
        if (glitchMaterial != null)
        {
            DestroyImmediate(glitchMaterial);
        }
    }
}