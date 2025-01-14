using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : UIBase
{
    public GameObject PausePanel;

    public Button retryButtonYes;
    public Button retryButtonNo;

    private void Start()
    {
        retryButtonYes.onClick.AddListener(Retry);
        retryButtonNo.onClick.AddListener(BackToGame);

        Invoke("GameClear", GameManager.Instance.maxGameTime);
    }
    private void Retry()
    {
        GameManager.Instance.uiManager.Hide<PauseUI>();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void BackToGame()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.uiManager.Hide<PauseUI>();
    }
}
