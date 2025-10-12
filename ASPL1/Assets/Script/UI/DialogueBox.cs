using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using System;

public class DialogueBox : MonoBehaviour
{

    //public Camera mainCamera;
    //public float targetFieldOfView;
    //public float originalFieldOfView;
    //public float viewChangeDuration;

    [SerializeField] protected string _dialogueKey = "???";
    public GameObject dialogueBox;
    [SerializeField] protected TMP_Text _dialogueText;
    [SerializeField] protected TMP_Text npcName;

    public float canTalkMaxDistance;

    public string[] _sentences; // 分割后的句子数组protected
    protected int _currentSentenceIndex = -1; // 当前显示的句子索引
    protected bool _isDialogActive = false;

    [SerializeField] protected RectTransform dialogRect; // 对话框的RectTransform组件


    [SerializeField] protected float DialogExpendDuration = 0.5f; // 动画持续时间（秒）


    protected float defaultTypeDuration = 0.07f;
    protected float typeDuration;
    [System.Serializable]
    protected struct changeDialogueDuration
    {
        public int targetIndex;//想要调整打字机速度的句子下标
        public float targetDuration;
    }

    [SerializeField] protected changeDialogueDuration[] CDS;

    protected Coroutine _typingCoroutine;

    [Header("FadeEffect")]

    [SerializeField] protected RectTransform characterIcon; // 角色胸像的RectTransform组件
    [SerializeField] protected MoveDirection enterDirection = MoveDirection.Left;
    [SerializeField] protected float moveDistance = 100f;
    [SerializeField] protected float animationDuration = 0.5f;
    public enum MoveDirection { Left, Right }

    protected Vector2 originalPosition;
    protected CanvasGroup canvasGroup;
    protected float originalAlpha;

    private Vector3 oraginalScale;

    protected virtual void Awake()
    {
        InitializeComponents();
    }

    protected virtual void Start()
    {
        typeDuration = defaultTypeDuration;
        oraginalScale = dialogRect.localScale;
        //dialogueBox.SetActive(false);

        LoadLocalizedDialogue();
    }

    protected virtual void Update()
    {
    }


    public void StartDialog()
    {
        GameManager.instance.isInputLocked = true;
        _isDialogActive = true;
        //StartCoroutine("StartDialogSequence");
        StartCoroutine(StartDialogSequence());
        StartCoroutine(AnimateEnter());
    }

    protected IEnumerator StartDialogSequence()
    {
        // 第一步：启动视野扩大协程，并等待其完成
        // yield return StartCoroutine(ExpandFieldOfViewCoroutine());

        // 第二步：视野扩大完成后，启动对话框动画协程
        yield return StartCoroutine(ScaleDialogCoroutine());

        // 标记对话激活
        //_isDialogActive = true;
    }

    // 对话框缩放动画协程（仅在视野扩大完成后调用）
    protected IEnumerator ScaleDialogCoroutine()
    {
        _dialogueText.gameObject.SetActive(true);
        npcName.gameObject.SetActive(true);

        // 先激活对话框（但保持缩放为0）
        dialogueBox.SetActive(true);
        //Vector3 oraginalScale = dialogRect.localScale;
        dialogRect.localScale = Vector3.zero;

        // 开始缩放动画
        float duration = DialogExpendDuration;
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            dialogRect.localScale = Vector3.Lerp(Vector3.zero, oraginalScale, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        dialogRect.localScale = oraginalScale;

        // 动画结束后开始显示对话
        _currentSentenceIndex = -1;
        ShowNextSentence();
    }

    #region Control Field of view

    // 视野扩大协程（返回 IEnumerator）
    //private IEnumerator ExpandFieldOfViewCoroutine()
    //{
    //    float startFOV = mainCamera.fieldOfView;
    //    float timeElapsed = 0f;

    //    while (timeElapsed < viewChangeDuration)
    //    {
    //        mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFieldOfView, timeElapsed / viewChangeDuration);
    //        timeElapsed += Time.deltaTime;
    //        yield return null;
    //    }
    //    mainCamera.fieldOfView = targetFieldOfView; // 确保最终值精确
    //}

    //private Coroutine _fovCoroutine; // 用于控制视野过渡的协程



    //// 线性恢复视野
    //public void RecoverFieldOfView()
    //{
    //    if (_fovCoroutine != null) StopCoroutine(_fovCoroutine);
    //    _fovCoroutine = StartCoroutine(LerpFieldOfView(mainCamera.fieldOfView, originalFieldOfView, viewChangeDuration));
    //}

    //// 通用的线性插值协程
    //private IEnumerator LerpFieldOfView(float startFOV, float targetFOV, float duration)
    //{
    //    float timeElapsed = 0f;
    //    while (timeElapsed < duration)
    //    {
    //        // 计算插值比例（0~1）
    //        float t = timeElapsed / duration;
    //        mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
    //        timeElapsed += Time.deltaTime;
    //        yield return null;
    //    }
    //    // 确保最终值精确
    //    mainCamera.fieldOfView = targetFOV;
    //}

    #endregion

    public void ShowNextSentence()
    {
        //StartCoroutine("DialogShake");
        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _currentSentenceIndex++;
        if (_currentSentenceIndex < _sentences.Length)
            _typingCoroutine = StartCoroutine(TypeSentence(_sentences[_currentSentenceIndex]));
        else
            EndDialog();
    }


    protected void ChangeTypeSpeed()
    {
        typeDuration = defaultTypeDuration;
        foreach (var setting in CDS)
        {
            if (setting.targetIndex == _currentSentenceIndex)
            {
                typeDuration = setting.targetDuration;
                break;
            }
        }
    }

    protected IEnumerator TypeSentence(string sentence)
    {
        ChangeTypeSpeed(); // 每次启动新句子时更新速度
        _dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            _dialogueText.text += letter;
            float currentDuration = typeDuration;
            yield return new WaitForSeconds(currentDuration);
        }
    }





    // 结束对话
    protected void EndDialog()
    {
        GameManager.instance.isInputLocked = false;
        StartCoroutine(AnimateExit());

        _sentences = Array.Empty<string>();

        // RecoverFieldOfView();
        OnCloseButtonClicked();
        //dialogueBox.SetActive(false);
        _isDialogActive = false;
    }



    protected void LoadLocalizedDialogue()
    {
        _typingCoroutine = null;
        dialogueBox.SetActive(false);//新增初始化项

        var operation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", _dialogueKey);
        operation.Completed += (op) =>
        {
            string fullText = op.Result;
            _sentences = fullText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        };

    }


    private System.Collections.IEnumerator ScaleDialog()
    {
        _dialogueText.gameObject.SetActive(true);
        npcName.gameObject.SetActive(true);

        float elapsedTime = 0f;
        Vector3 startScale = Vector3.zero; // 初始大小（隐藏）
        Vector3 targetScale = Vector3.one * 3; // 目标大小（正常大小）

        while (elapsedTime < DialogExpendDuration)
        {
            // 计算当前时间的插值比例（0到1之间）
            float t = elapsedTime / DialogExpendDuration;
            // 线性插值缩放值
            dialogRect.localScale = Vector3.Lerp(startScale, targetScale, t);
            // 累加时间
            elapsedTime += Time.deltaTime;
            // 等待下一帧
            yield return null;
        }

        // 确保最终状态准确
        dialogRect.localScale = targetScale;
    }


    public void OnCloseButtonClicked()
    {
        StartCoroutine("ShrinkDialog");
    }

    private System.Collections.IEnumerator ShrinkDialog()
    {
        _dialogueText.gameObject.SetActive(false);
        npcName.gameObject.SetActive(false);

        float elapsedTime = 0f;
        Vector3 startScale = dialogRect.localScale;
        Vector3 targetScale = Vector3.zero;

        while (elapsedTime < DialogExpendDuration)
        {
            float t = elapsedTime / DialogExpendDuration;
            dialogRect.localScale = Vector3.Lerp(startScale, targetScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        dialogRect.localScale = targetScale;
        dialogueBox.SetActive(false); // 动画结束后隐藏对话框
    }

    #region charaterIconFade
    protected virtual void InitializeComponents()
    {
        originalPosition = characterIcon.anchoredPosition;
        canvasGroup = characterIcon.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = characterIcon.gameObject.AddComponent<CanvasGroup>();
        originalAlpha = canvasGroup.alpha;
    }


    protected virtual IEnumerator AnimateEnter()
    {
        Vector2 startPosition = CalculateStartPosition(enterDirection);

        characterIcon.anchoredPosition = startPosition;
        canvasGroup.alpha = 0f;

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = EaseOutQuint(elapsed / animationDuration);

            characterIcon.anchoredPosition = Vector2.Lerp(
                startPosition,
                originalPosition,
                t
            );

            canvasGroup.alpha = Mathf.Lerp(0f, originalAlpha, t);
            yield return null;
        }

        characterIcon.anchoredPosition = originalPosition;
        canvasGroup.alpha = originalAlpha;
    }

    protected virtual IEnumerator AnimateExit()
    {
        MoveDirection exitDirection = GetOppositeDirection(enterDirection);
        Vector2 endPosition = CalculateStartPosition(exitDirection);

        Vector2 startPosition = characterIcon.anchoredPosition;
        float startAlpha = canvasGroup.alpha;

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = EaseInQuint(elapsed / animationDuration);

            characterIcon.anchoredPosition = Vector2.Lerp(
                startPosition,
                endPosition,
                t
            );

            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);
            yield return null;
        }

        characterIcon.anchoredPosition = endPosition;
        canvasGroup.alpha = 0f;
    }

    private Vector2 CalculateStartPosition(MoveDirection direction)
    {
        Vector2 position = originalPosition;
        position.x += direction == MoveDirection.Left ? -moveDistance : moveDistance;
        return position;
    }

    private MoveDirection GetOppositeDirection(MoveDirection direction)
    {
        return direction == MoveDirection.Left ? MoveDirection.Right : MoveDirection.Left;
    }

    // 缓动函数
    private float EaseOutQuint(float t) => 1 - Mathf.Pow(1 - t, 5);
    private float EaseInQuint(float t) => Mathf.Pow(t, 5);


    #endregion
}
