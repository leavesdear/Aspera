using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class Enemy_Flos : Enemy
{
    [Header("Attack Info")]
    public float SpikeReleseDuration;
    public GameObject rootPrefab;
    public GameObject warningPrefab;
    public GameObject groundRootPrefab;
    public float warningDuration;
    public bool isWarning = false;
    public float keepRootTime;

    public Transform parent;

    public FlosIdleState idleState { get; private set; }
    public FlosHideState hideState { get; private set; }
    public FlosDieState dieState { get; private set; }
    public FlosHitState hitState { get; private set; }

    public bool beFound = false;
    public GameObject healthBar;

    public float hitForce;
    public float hitTime = .3f;


    protected override void Awake()
    {
        base.Awake();
        parent = transform.parent;
        healthBar.SetActive(false);


        idleState = new FlosIdleState(this, stateMachine, "idle", this);
        hideState = new FlosHideState(this, stateMachine, "hide", this);
        dieState = new FlosDieState(this, stateMachine, "die", this);
        hitState = new FlosHitState(this, stateMachine, "hit", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(hideState);
    }

    protected override void Update()
    {
        base.Update();
        if (gameObject.GetComponent<EnemyStats>().currentHealth < gameObject.GetComponent<EnemyStats>().GetMaxHealthValue() && !beFound)
        {
            beFound = true;
        }
    }

    public void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(dieState);
    }

    private GameObject warning;

    public void ReleseRoot(Vector3 relesePosition)
    {
        if (warning != null)
            Destroy(warning);
        GameObject root = Instantiate(rootPrefab, relesePosition, Quaternion.identity, parent);
        isWarning = false;

        StartCoroutine(WaitForDestoryRoot(root));
    }

    public void StartWarning()
    {
        Vector3 playerPosition = PlayerManger.instance.player.transform.position;
        warning = Instantiate(warningPrefab, playerPosition, Quaternion.identity, parent);
        isWarning = true;
        StartCoroutine(WaitForReleseRoot(playerPosition));
    }

    private IEnumerator WaitForDestoryRoot(GameObject root)
    {
        yield return new WaitForSeconds(keepRootTime);
        Destroy(root);
    }

    private IEnumerator WaitForReleseRoot(Vector3 relesePosition)
    {
        yield return new WaitForSeconds(warningDuration);
        ReleseRoot(relesePosition);
    }

    public override void damageEffect()
    {
        base.damageEffect();

        if (GetComponent<EnemyStats>().currentHealth > 0)
        {
            stateMachine.ChangeState(hitState);
        }
    }
}
