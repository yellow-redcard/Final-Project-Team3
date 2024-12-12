using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TitleScene : MonoBehaviour
{
    [Header("Button")]
    public Button startGameButton;
    public Button upgradeButton;
    public Button opotionButton;
    public Button quitGameButton;

    [Header("Panel")]
    public GameObject lobbyUI;
    public GameObject optionPanel;
    public GameObject upgradePanel;

    [Header("BakcBtn")]
    public Button backToTitleUpgrade;
    public Button backToTitleOPtion;
    private void Start()
    {
        startGameButton.onClick.AddListener(StartGame);
        upgradeButton.onClick.AddListener(ShowUpgradeMenu);
        opotionButton.onClick.AddListener(ShowOptionMenu);
        quitGameButton.onClick.AddListener(QuitGame);

        backToTitleUpgrade.onClick.AddListener(CloseUpgradeMenu);
        backToTitleOPtion.onClick.AddListener(CloseOptionMenu);
    }
    private void StartGame()
    {
        SceneManager.LoadScene("HynuScene");
    }
    private void ShowUpgradeMenu()
    {
        lobbyUI.SetActive(false);
        upgradePanel.SetActive(true);
    }
    void CloseUpgradeMenu()
    {
        upgradePanel.SetActive(false); // 강화 패널 비활성화
        lobbyUI.SetActive(true);
    }
    private void ShowOptionMenu()
    {
        optionPanel.SetActive(true);
        lobbyUI.SetActive(false);
    }
    void CloseOptionMenu()
    {
        optionPanel.SetActive(false); // 옵션 패널 비활성화
        lobbyUI.SetActive(true);
    }

    private void QuitGame()
    {
        Application.Quit();
    }


}
