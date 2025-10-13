using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyState : MonoBehaviour
{
    protected Enemy enemyBase;
    protected EnemyStateMechine stateMechine;
    private string setBoolName;
    protected float stateTimer;
    protected bool triggerCalled;
    protected Rigidbody2D rb;
    protected Player player;

    protected bool enemyFliped = false;
    protected float enemyHealthPercent;

    #region PathFinding
    protected Seeker seeker;
    protected int currentIndex = 0;//路径点索引
    protected List<Vector3> pathPointList = new List<Vector3>();//路径点列表
    protected float pathGenerateInterval = 0.5f;//每0.05秒生成一次路径
    protected float pathGenerateTimer = 0f;
    #endregion

    public EnemyState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMechine = _stateMechine;
        this.setBoolName = _setBoolName;
    }

    public virtual void Awake()
    {
        player = enemyBase.player;
    }

    public virtual void Start()
    {
        //player = new Player();
    }

    public virtual void Enter()
    {
        rb = enemyBase.rb;
        triggerCalled = false;
        enemyBase.anim.SetBool(setBoolName, true);
        player = PlayerManger.instance.player;
        seeker = enemyBase.seeker;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        enemyHealthPercent = enemyBase.GetComponent<EnemyStats>().currentHealth / enemyBase.GetComponent<EnemyStats>().GetMaxHealthValue();
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(setBoolName, false);
        //enemyBase.AssignLastAnimName(setBoolName);
    }

    public void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }


    public void AutoPath()
    {
        if (player == null)//玩家不能为空
            return;
        pathGenerateTimer += Time.deltaTime;


        //间隔一定时间来获取路径点
        if (pathGenerateTimer >= pathGenerateInterval)
        {
            GeneratePath(player.transform.position);
            pathGenerateTimer = 0;//重置计时器
        }
        //当路径点列表为空时,进行路径计算
        if (pathPointList == null || pathPointList.Count <= 0 || currentIndex >= pathPointList.Count)
        {
            GeneratePath(player.transform.position);
        }//当敌人到达当前路径点时，递增索引currentIndex并进行路径计算
        else if (currentIndex < pathPointList.Count)
        {
            if (Vector2.Distance(enemyBase.transform.position, pathPointList[currentIndex]) <= 0.1f)
            {
                currentIndex++;
                if (currentIndex >= pathPointList.Count)
                {
                    GeneratePath(player.transform.position);
                }
            }

        }
    }

    public void GeneratePath(Vector3 target)
    {
        seeker.StartPath(enemyBase.transform.position, target, Path =>
        {
            pathPointList = Path.vectorPath;
            currentIndex = 0;
        });
    }

    //以上为未修改代码

}
