using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [Header("Button")]
    public Button retryButtonYes;
    public Button retryButtonNo;
    public Button mainButton;

    [Header("Panel")]
    public GameObject gameOverPanel;
    public GameObject gameClearPanel;

    private void Start()
    {
        retryButtonYes.onClick.AddListener(Retry);
        retryButtonNo.onClick.AddListener(BackToMenu);
        mainButton.onClick.AddListener(BackToMenu);

        Invoke("GameClear", GameManager.Instance.maxGameTime);
    }
    private void Retry()
    {
        gameOverPanel.SetActive(false);
        gameClearPanel.SetActive(false);

        Time.timeScale = 1.0f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void BackToMenu()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        GameManager.Instance.uiManager.Show<GameOverUI>();
    }

    public void GameClear()
    {
        Time.timeScale = 0.0f;
        gameClearPanel.SetActive(true);
    }
}
