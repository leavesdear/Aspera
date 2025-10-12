using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialog : DialogueBox
{
    [SerializeField] private Player player;
    [SerializeField] private Transform npc;

    protected override void Start()
    {
        base.Start();
        player = PlayerManger.instance.player;
    }

    protected override void Update()
    {
        base.Update();
        if (Vector2.Distance(npc.position, player.transform.position) < canTalkMaxDistance && Input.GetKeyDown(KeyCode.Z))
        {
            if (!_isDialogActive)
                StartDialog();
            else
                ShowNextSentence();
        }
    }
}
