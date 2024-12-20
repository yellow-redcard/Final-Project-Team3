using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static SkillManager;

public class LevelUpUI : UIBase
{
    public void ShowLevelUpUI()
    {
        var upgradeOptions = GameManager.Instance.skillManager.GetUpgradeOptions();

        // UI에 옵션 표시
        foreach (var option in upgradeOptions)
        {
            Debug.Log($"업그레이드 선택지: 스킬 {option.Item1}, 옵션: {option.Item2}");
        }

        // 예시: 플레이어가 첫 번째 옵션을 선택한 경우
        HandleSkillUpgrade(upgradeOptions[0].Item1, upgradeOptions[0].Item2);
    }
    private void HandleSkillUpgrade(SkillType skillType, string option)
    {
        GameManager.Instance.skillManager.UpgradeSkill(skillType, option);
    }
    public void CloseUI()
    {
        GameManager.Instance.uiManager.Hide<LevelUpUI>();
        Time.timeScale = 1.0f;
    }
}
