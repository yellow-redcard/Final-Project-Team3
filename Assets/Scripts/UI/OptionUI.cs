using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : UIBase
{
    public Button backToTitle;

    private void Start()
    { 
        backToTitle.onClick.AddListener(CloseOptionMenu);
    }
    void CloseOptionMenu()
    {
        TitleManager.Instance.uiManager.Hide<OptionUI>();
        TitleManager.Instance.uiManager.Show<TitleUI>();
    }
}
