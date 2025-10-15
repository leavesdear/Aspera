
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using System;

public class DialogueBox : MonoBehaviour
{
    private float speakTimer;
    public float speakDuration;
    private int counter = 1;
    public Animator anim;

    [Header("Dialog Settings")]
    protected string _dialogueKey = "Aspera5";
    public GameObject dialogueBox;
    [SerializeField] protected TMP_Text _dialogueText;
    [SerializeField] protected float DialogExpandDuration = 0.5f;
    public float canTalkMaxDistance;

    [Header("Typing Settings")]
    [SerializeField] protected float defaultTypeDuration = 0.07f;
    [SerializeField] protected RectTransform dialogRect;

    [System.Serializable]
    protected struct ChangeDialogueDuration
    {
        public int targetIndex;
        public float targetDuration;
    }

    [Header("Custom Typing Speeds")]
    [SerializeField] protected ChangeDialogueDuration[] customDurations;

    protected string[] _sentences;
    protected int _currentSentenceIndex = -1;

    protected float typeDuration;
    protected Coroutine _typingCoroutine;
    protected Coroutine _closeCoroutine;
    protected Coroutine _openCoroutine;
    protected Vector3 originalScale;

    public bool _isDialogActive = false;
    protected bool _isLoadingDialogue = false;
    protected bool _isClosing = false; // 新增：标记是否正在关闭

    protected virtual void Awake()
    {
        if (dialogRect == null && dialogueBox != null)
        {
            dialogRect = dialogueBox.GetComponent<RectTransform>();
        }

        if (dialogRect == null)
        {
            dialogRect = GetComponent<RectTransform>();
        }
    }

    protected virtual void Start()
    {
        typeDuration = defaultTypeDuration;
        originalScale = dialogRect.localScale;
        dialogueBox.SetActive(false);
        speakTimer = speakDuration;
        LoadLocalizedDialogue();
    }

    public virtual void StartDialog()
    {
        if (_isDialogActive || _isLoadingDialogue || _isClosing) return;

        AudioManager.instance.PlaySFX(0);

        _isDialogActive = true;
        // 确保文本在开始前为空
        _dialogueText.text = "";

        if (_openCoroutine != null)
            StopCoroutine(_openCoroutine);

        _openCoroutine = StartCoroutine(StartDialogSequence());
    }

    protected IEnumerator StartDialogSequence()
    {
        // 确保对话框处于激活状态但不可见
        dialogueBox.SetActive(true);
        _dialogueText.gameObject.SetActive(true);

        // 重置缩放
        dialogRect.localScale = Vector3.zero;

        // 确保文本为空
        _dialogueText.text = "";

        // 缩放动画
        float timeElapsed = 0f;
        while (timeElapsed < DialogExpandDuration)
        {
            dialogRect.localScale = Vector3.Lerp(
                Vector3.zero,
                originalScale,
                timeElapsed / DialogExpandDuration
            );

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        dialogRect.localScale = originalScale;
        _currentSentenceIndex = -1;
        ShowNextSentence();

        _openCoroutine = null;
    }

    public void ShowNextSentence()
    {
        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _currentSentenceIndex++;
        ChangeMouthAnimation();

        if (_currentSentenceIndex < _sentences.Length)
            _typingCoroutine = StartCoroutine(TypeSentence(_sentences[_currentSentenceIndex]));
        else
            EndDialog();
    }

    protected virtual void Update()
    {
        if (speakTimer > 0)
        {
            speakTimer -= Time.deltaTime;
        }
        else if (counter > 0)
        {
            counter--;
            speakTimer = speakDuration;
            anim.SetBool("speak", false);
            anim.SetBool("quite", true);
        }
    }

    private void ChangeMouthAnimation()
    {
        anim.SetBool("speak", true);
        anim.SetBool("quite", false);
        counter = 1;
    }

    protected IEnumerator TypeSentence(string sentence)
    {
        UpdateTypeSpeed();
        _dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            _dialogueText.text += letter;
            AudioManager.instance.PlaySFX(2);
            yield return new WaitForSeconds(typeDuration);
        }
    }

    protected void UpdateTypeSpeed()
    {
        typeDuration = defaultTypeDuration;

        foreach (var setting in customDurations)
        {
            if (setting.targetIndex == _currentSentenceIndex)
            {
                typeDuration = setting.targetDuration;
                break;
            }
        }
    }

    public void EndDialog()
    {
        if (_isClosing) return;

        _isClosing = true;

        if (_closeCoroutine != null)
        {
            StopCoroutine(_closeCoroutine);
        }

        _closeCoroutine = StartCoroutine(CloseDialog());

        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
            _typingCoroutine = null;
        }

        _dialogueText.text = "";
        _sentences = Array.Empty<string>();
        _isDialogActive = false;
    }

    private IEnumerator CloseDialog()
    {
        _dialogueText.gameObject.SetActive(false);

        float elapsedTime = 0f;
        Vector3 startScale = dialogRect.localScale;
        Vector3 targetScale = Vector3.zero;

        while (elapsedTime < DialogExpandDuration)
        {
            dialogRect.localScale = Vector3.Lerp(
                startScale,
                targetScale,
                elapsedTime / DialogExpandDuration
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        dialogRect.localScale = targetScale;
        dialogueBox.SetActive(false);
        _closeCoroutine = null;
        _isClosing = false;
    }

    protected void LoadLocalizedDialogue()
    {
        var operation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", _dialogueKey);
        operation.Completed += (op) =>
        {
            if (op.IsDone && op.Result != null)
            {
                string fullText = op.Result;
                _sentences = fullText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                _isLoadingDialogue = false;
            }
        };
    }

    public void OnCloseButtonClicked()
    {
        if (_isDialogActive)
        {
            EndDialog();
        }
    }

    public void SetCharaterDialogActive(bool _isActive)
    {
        dialogueBox.SetActive(_isActive);
    }

    // 完全重写的StartNewDialog方法
    public virtual void StartNewDialog(string newKey)
    {
        // 如果正在关闭，等待关闭完成
        if (_isClosing)
        {
            StartCoroutine(WaitForCloseThenStartNew(newKey));
            return;
        }

        // 停止所有可能正在运行的协程
        if (_openCoroutine != null)
        {
            StopCoroutine(_openCoroutine);
            _openCoroutine = null;
        }

        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
            _typingCoroutine = null;
        }

        if (_closeCoroutine != null)
        {
            StopCoroutine(_closeCoroutine);
            _closeCoroutine = null;
        }

        // 立即关闭对话框并清空文本
        dialogueBox.SetActive(false);
        _dialogueText.text = "";

        // 设置新键并标记为正在加载
        _dialogueKey = newKey;
        _isLoadingDialogue = true;
        _isDialogActive = false;

        // 加载新对话
        LoadLocalizedDialogueWithCallback(() =>
        {
            _isLoadingDialogue = false;
            StartDialog();
        });
    }

    // 等待关闭完成后开始新对话
    protected IEnumerator WaitForCloseThenStartNew(string newKey)
    {
        while (_isClosing)
        {
            yield return null;
        }

        StartNewDialog(newKey);
    }

    // 修改后的LoadLocalizedDialogue方法（添加回调参数）
    protected void LoadLocalizedDialogueWithCallback(Action onComplete = null)
    {
        var operation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", _dialogueKey);
        operation.Completed += (op) =>
        {
            if (op.IsDone && op.Result != null)
            {
                string fullText = op.Result;
                _sentences = fullText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                onComplete?.Invoke();
            }
            else
            {
                _isLoadingDialogue = false;
            }
        };
    }
}