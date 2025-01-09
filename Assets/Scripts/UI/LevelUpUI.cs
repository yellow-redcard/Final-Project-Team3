using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class LevelUpUI : UIBase
{
    [SerializeField] private Text levelUpText; // 상단 레벨업 안내 텍스트
    [SerializeField] private Button[] skillButtons; // 버튼 배열
    private List<SkillData> allSkills; // 모든 스킬 데이터

    public void ConfigureButtons(List<SkillData> skills)
    {
        // 업그레이드 가능한 스킬과 새로 해금 가능한 스킬을 분리
        var upgradableSkills = skills.Where(s => s.level < s.maxLevel).ToList();

        // 업그레이드 가능한 스킬만 사용
        allSkills = upgradableSkills.OrderBy(x => Random.value).ToList(); // 랜덤 순서로 정렬

        for (int i = 0; i < skillButtons.Length; i++)
        {
            // 모든 이전 이벤트 리스너 제거
            skillButtons[i].onClick.RemoveAllListeners();

            if (i < allSkills.Count)
            {
                SkillData skillData = allSkills[i];

                // 스킬 이름과 레벨 정보를 버튼 텍스트에 표시
                string buttonText = $"{skillData.skillName}\nLv {skillData.level}/{skillData.maxLevel}";
                var buttonTextComponent = skillButtons[i].GetComponentInChildren<Text>();

                if (buttonTextComponent != null)
                {
                    buttonTextComponent.text = buttonText;
                }

                // 버튼 클릭 이벤트 등록
                skillButtons[i].onClick.AddListener(() =>
                {
                    OnSkillSelected(skillData); // 스킬 처리
                    CloseUI(); // UI 닫기
                });

                // 버튼 활성화
                skillButtons[i].gameObject.SetActive(true);
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
    public void OnSkillSelected(SkillData selectedSkill)
    {
        if (selectedSkill == null)
        {
            return;
        }

        // 스킬 업그레이드 또는 해금 처리
        GameManager.Instance.skillManager.UpgradeOrUnlockSkill(selectedSkill);

        // 선택된 스킬의 레벨 정보 업데이트
        UpdateSelectedSkillLevel(selectedSkill);

        // UI 닫기
        CloseUI();
    }
    private void UpdateSelectedSkillLevel(SkillData selectedSkill)
    {
        for (int i = 0; i < allSkills.Count; i++)
        {
            if (allSkills[i] == selectedSkill)
            {
                string buttonText = $"{selectedSkill.skillName}\nLv {selectedSkill.level}/{selectedSkill.maxLevel}";

                var buttonTextComponent = skillButtons[i].GetComponentInChildren<Text>();
                buttonTextComponent.text = buttonText;
                break;
            }
        }
    }

    /// <summary>
    /// 레벨업 UI를 닫고 게임을 재개합니다.
    /// </summary>
    public void CloseUI()
    {
        // 모든 버튼 비활성화
        foreach (var button in skillButtons)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners(); // 이벤트 리스너 제거
        }

        Time.timeScale = 1f; // 게임 재개
        gameObject.SetActive(false); // UI 비활성화
    }
}
