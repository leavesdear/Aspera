using System.Collections;
using System.Collections.Generic;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

public class FlosRootSkill : Skill
{
    private GameObject warning;
    public Enemy_Flos enemy;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public void ReleseRoot(Vector3 relesePosition)
    {
        if (warning != null)
            Destroy(warning);
        GameObject root = Instantiate(enemy.rootPrefab, relesePosition, Quaternion.identity, enemy.parent);
        enemy.isWarning = false;

        StartCoroutine(WaitForDestoryRoot(root));
    }

    public void StartWarning()
    {
        Vector3 playerPosition = PlayerManger.instance.player.transform.position;
        warning = Instantiate(enemy.warningPrefab, playerPosition, Quaternion.identity, enemy.parent);
        enemy.isWarning = true;
        StartCoroutine(WaitForReleseRoot(playerPosition));
    }

    public override void UseSkill()
    {
        base.UseSkill();
        StartWarning();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private IEnumerator WaitForDestoryRoot(GameObject root)
    {
        yield return new WaitForSeconds(enemy.keepRootTime);
        Destroy(root);
    }

    private IEnumerator WaitForReleseRoot(Vector3 relesePosition)
    {
        yield return new WaitForSeconds(enemy.warningDuration);
        ReleseRoot(relesePosition);
    }
}
