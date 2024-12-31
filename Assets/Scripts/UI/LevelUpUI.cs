using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelUpUI :    UIBase
{
    [SerializeField] private Text levelUpText; // 상단 레벨업 안내 텍스트
    [SerializeField] private Button[] skillButtons; // 버튼 배열

    /// <summary>
    /// 레벨업 UI 버튼과 데이터를 초기화합니다.
    /// </summary>
    /// <param name="skills">업그레이드 가능한 스킬 데이터 리스트</param>
    public void ConfigureButtons(List<SkillData> skills)
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (i < skills.Count)
            {
                SkillData skillData = skills[i];

                // 버튼 텍스트 설정
                string buttonText = skillData.level < skillData.maxLevel
                    ? $"{skillData.skillName}\n[업그레이드 가능]\nLv {skillData.level}/{skillData.maxLevel}"
                    : $"{skillData.skillName}\n[새로운 스킬 해금]";

                skillButtons[i].GetComponentInChildren<Text>().text = buttonText;

                // 버튼 클릭 이벤트 설정
                skillButtons[i].onClick.RemoveAllListeners(); // 기존 이벤트 제거
                skillButtons[i].onClick.AddListener(() => OnSkillSelected(skillData)); // 새로운 이벤트 추가
                skillButtons[i].gameObject.SetActive(true); // 버튼 활성화
            }
            else
            {
                // 남는 버튼 비활성화
                skillButtons[i].gameObject.SetActive(false);
            }
        }

        // UI 활성화
        gameObject.SetActive(true);
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

        Debug.Log($"[OnSkillSelected] 선택된 스킬: {selectedSkill.skillName}");

        // 스킬 업그레이드 또는 해금 처리
        GameManager.Instance.skillManager.UpgradeOrUnlockSkill(selectedSkill);

        Debug.Log($"[LevelUpUI] '{selectedSkill.skillName}' 선택 완료!");

        // UI 닫기
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
