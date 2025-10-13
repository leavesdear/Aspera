using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDialog : DialogueBox
{
    public GameObject bossHealthBar;
    public Player player;
    public Enemy_Boss boss;
    protected override void Awake()
    {
        base.Awake();
        _dialogueKey = "凯利斯1";
        // PlayerManger.instance.player.GetComponent<PlayerStats>().canReviveCounter = 1;
    }

    protected override void Start()
    {
        base.Start();
        player = PlayerManger.instance.player;
        //boss = EnemyManager.instance.boss;
        npcName.text = "凯利斯";
    }

    protected override void Update()
    {
        base.Update();
        if (player.anim.GetBool("die") && player.playerStats.canReviveCounter > 0 && _dialogueKey == "凯利斯1")
        {

            boss.stateMachine.ChangeState(boss.idleState);
            //StartCoroutine(GameManager.instance.WaitForSecond(5f));

            _dialogueKey = "凯利斯2";
            LoadLocalizedDialogue();

            bossHealthBar.SetActive(false);

            StartDialog();
            return;
        }
        if (_dialogueKey == "凯利斯2" && !_isDialogActive && player.playerStats.canReviveCounter > 0)
        {
            PlayerManger.instance.player.playerStats.RevivePlayer();
            boss.stateMachine.ChangeState(boss.battleState);
            bossHealthBar.SetActive(true);
        }


        if (_currentSentenceIndex == 1 && _dialogueKey == "凯利斯1")
        {
            bossHealthBar.SetActive(true);
            boss.stateMachine.ChangeState(boss.battleState);
            _currentSentenceIndex = -1;
            AudioManager.instance.playBgm = true;
            AudioManager.instance.PlayBGM(0);
        }
        else if (_isDialogActive && Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextSentence();
        }

    }
}
