using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelUpUI : UIBase
{
    [SerializeField] private Text levelUpText; // 상단 레벨업 안내 텍스트
    [SerializeField] private Button[] skillButtons; // 버튼 배열

    public void ConfigureButtons(List<SkillData> skills)
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (i < skills.Count)
            {
                SkillData skillData = skills[i];

                string buttonText = skillData.level < skillData.maxLevel
                    ? $"{skillData.skillName}\n[업그레이드 가능]\nLv {skillData.level}/{skillData.maxLevel}"
                    : $"{skillData.skillName}\n[새로운 스킬 해금]";

                var buttonTextComponent = skillButtons[i].GetComponentInChildren<Text>();
                if (buttonTextComponent == null)
                {
                    Debug.LogError($"[LevelUpUI] 버튼 {i}에 Text 컴포넌트가 없습니다! Text 컴포넌트를 추가하세요.");
                }
                else
                {
                    buttonTextComponent.text = buttonText;
                }

                // 버튼 활성화
                if (!skillButtons[i].gameObject.activeSelf)
                {
                    skillButtons[i].gameObject.SetActive(true);
                }
            }
            else
            {
                skillButtons[i].gameObject.SetActive(false); // 스킬 데이터가 없으면 버튼 비활성화
            }
        }
    }

    /// <summary>
    /// 버튼 클릭 시 호출되는 메서드. 선택된 스킬을 처리합니다.
    /// </summary>
    /// <param name="selectedSkill">클릭된 버튼에 연결된 스킬 데이터</param>
    private void OnSkillSelected(SkillData selectedSkill)
    {
        if (selectedSkill == null)
        {
            Debug.LogError("[LevelUpUI] 선택된 스킬 데이터가 null입니다!");
            return;
        }

        Debug.Log($"[LevelUpUI] 선택된 스킬: {selectedSkill.skillName}");

        // 스킬 업그레이드 또는 해금 처리
        GameManager.Instance.skillManager.UpgradeOrUnlockSkill(selectedSkill);

        // UI 닫기 및 게임 재개
        CloseUI();
    }

    /// <summary>
    /// 레벨업 UI를 닫고 게임을 재개합니다.
    /// </summary>
    public void CloseUI()
    {
        Time.timeScale = 1f; // 게임 재개
        gameObject.SetActive(false); // UI 비활성화
    }
}
