using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class PlayerDialog : DialogueBox
{
    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Start()
    {
        base.Start();
        //npcName.text = "阿丝佩拉";
        StartDialog();
    }

    protected override void Update()
    {
        base.Update();
        if (_isDialogActive && Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextSentence();
        }
        if (_currentSentenceIndex == 2)
        {
            GameManager.instance.WaitForSecond(.5f);
            DialogManager.instance.boss.StartDialog();
            _currentSentenceIndex = -1;
        }
    }
}
