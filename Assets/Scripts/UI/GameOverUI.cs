using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : UIBase
{
    public GameObject gameOverPanel;

    public Button retryButtonYes;
    public Button retryButtonNo;

    private void Start()
    {
        retryButtonYes.onClick.AddListener(Retry);
        retryButtonNo.onClick.AddListener(BackToMenu);

        Invoke("GameClear", GameManager.Instance.maxGameTime);
    }
    private void Retry()
    {
        GameManager.Instance.uiManager.Hide<GameOverUI>();

        Time.timeScale = 1.0f;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void BackToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("TitleScene");
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        GameManager.Instance.uiManager.Show<GameOverUI>();
    }
}
