using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnforceUI : UIBase
{
    public Button backToTitle;

    private void Start()
    {
        backToTitle.onClick.AddListener(CloseEnforceMenu);
    }
    void CloseEnforceMenu()
    {
        TitleManager.Instance.uiManager.Hide<EnforceUI>();
        TitleManager.Instance.uiManager.Show<TitleUI>();
    }
}
