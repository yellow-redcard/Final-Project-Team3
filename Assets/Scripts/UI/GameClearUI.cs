using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearUI : UIBase
{
    public GameObject gameClearPanel;

    public Button mainButton;

    private void Start()
    {
        mainButton.onClick.AddListener(BackToMenu);

        Invoke("GameClear", GameManager.Instance.maxGameTime);
    }
    private void BackToMenu()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void GameClear()
    {
        Time.timeScale = 0.0f;
        GameManager.Instance.uiManager.Show<GameClearUI>();
    }
}
