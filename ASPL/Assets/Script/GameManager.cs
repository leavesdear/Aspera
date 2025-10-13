using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // 拖拽赋值
    private bool isPaused;

    public bool isInputLocked = false;

    public static GameManager instance;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    void Update()
    {
        GetGamePause();
    }

    private void GetGamePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        isInputLocked = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;



        // 解锁鼠标（适用于 PC 游戏）
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        isInputLocked = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public IEnumerator WaitForSecond(float _waitTime)
    {
        yield return new WaitForSeconds(_waitTime);
    }
}
