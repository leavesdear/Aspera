using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clash_Skill : Skill
{
    public GameObject prefab;
    private GameObject instance;
    public float existTime = 5f;

    public override void UseSkill()
    {
        base.UseSkill();

        // 清理旧实例
        if (instance != null)
        {
            Destroy(instance);
            instance = null; // 关键：必须清空引用
        }

        // 生成新实例
        instance = Instantiate(prefab, boss.transform.position, Quaternion.identity);
        //instance.GetComponentInChildren<Transform>().position = new Vector3(boss.transform.position.x, boss.transform.position.y, instance.GetComponentInChildren<Transform>().position.z);

        // 启动单一销毁协程
        StartCoroutine(DestroyAfterTime(existTime));
    }

    private IEnumerator DestroyAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (instance != null)
        {
            Destroy(instance);
            instance = null; // 必须清空引用
        }
    }

    // 移除Update中的计时逻辑
    protected override void Update()
    {
        base.Update();
        // 原计时器逻辑已移除
    }

}
