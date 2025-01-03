using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUI : UIBase
{
    public Button startGameButton;
    public Button EnforceButton;
    public Button opotionButton;
    public Button quitGameButton;

    private void Start()
    {
        startGameButton.onClick.AddListener(StartGame);
        EnforceButton.onClick.AddListener(ShowEnforceMenu);
        opotionButton.onClick.AddListener(ShowOptionMenu);
        quitGameButton.onClick.AddListener(QuitGame);
    }
    private void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    private void ShowEnforceMenu()
    {
        TitleManager.Instance.uiManager.Hide<TitleUI>();
        TitleManager.Instance.uiManager.Show<EnforceUI>();
    }
    private void ShowOptionMenu()
    {
        TitleManager.Instance.uiManager.Hide<TitleUI>();
        TitleManager.Instance.uiManager.Show<OptionUI>();
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
