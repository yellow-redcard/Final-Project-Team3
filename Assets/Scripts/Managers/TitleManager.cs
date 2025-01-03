using UnityEngine;

public class TitleManager : MonoSingleton<TitleManager>
{
    public UIManager uiManager;

    private void Start()
    {
        uiManager.init();
        uiManager.Show<TitleUI>();
    }
}
