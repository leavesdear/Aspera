using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CartoonFX;
public class Smash_skill : Skill
{
    [Header("特效设置")]
    public GameObject cfxrPrefab;
    public float destoryDelay;

    [Header("生成设置")]
    public GameObject prefab;
    public int spawnCount = 10;
    public float radius = 5f;
    public bool drawGizmos = true;
    public Transform parent;

    [Header("销毁设置")]
    public float fadeDuration = 2f; // 渐隐持续时间（总销毁时间）

    private Vector3 bossPosition;

    private GameObject effect;

    protected override void Start()
    {
        base.Start();
    }

    public void SpawnInCircle()
    {
        if (prefab == null)
        {
            Debug.LogError("未指定预制体！");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 randomPos = GetRandomPositionInCircle();
            Vector3 spawnPos = bossPosition + new Vector3(randomPos.x, randomPos.y, 0);

            GameObject instance = Instantiate(prefab, spawnPos, Quaternion.identity, parent);

            // 启动渐隐协程
            StartCoroutine(FadeAndDestroy(instance));
        }
    }

    Vector2 GetRandomPositionInCircle()
    {
        float randomRadius = Mathf.Sqrt(Random.value) * radius;
        float angle = Random.Range(0, 2 * Mathf.PI);
        return new Vector2(
            randomRadius * Mathf.Cos(angle),
            randomRadius * Mathf.Sin(angle)
        );
    }

    IEnumerator FadeAndDestroy(GameObject target)
    {
        // 获取所有渲染组件
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        Material[] materials = new Material[renderers.Length];
        Color[] initialColors = new Color[renderers.Length];

        // 初始化材质和颜色
        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].material;
            initialColors[i] = materials[i].color;
        }

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            for (int i = 0; i < materials.Length; i++)
            {
                Color newColor = initialColors[i];
                newColor.a = alpha;
                materials[i].color = newColor;
            }

            yield return null;
        }

        Destroy(target);
        //Destroy(effect);
    }

    void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(bossPosition, radius);
    }

    public override void UseSkill()
    {
        base.UseSkill();

        bossPosition = EnemyManager.instance.boss.transform.position;

        effect = Instantiate(cfxrPrefab, bossPosition, Quaternion.identity);
        effect.transform.localScale = Vector3.one * 15;

        SpawnInCircle();
    }


}
