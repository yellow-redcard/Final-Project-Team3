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
        var unlockableSkills = skills.Where(s => s.level == s.maxLevel).ToList();

        // 업그레이드 가능한 스킬과 새로 해금 가능한 스킬을 섞어서 버튼에 표시
        allSkills = upgradableSkills.Concat(unlockableSkills).ToList();
        allSkills = allSkills.OrderBy(x => Random.value).ToList(); // 랜덤 순서로 정렬

        for (int i = 0; i < skillButtons.Length; i++)
        {
            // 모든 이전 이벤트 리스너 제거
            skillButtons[i].onClick.RemoveAllListeners();

            if (i < allSkills.Count)
            {
                SkillData skillData = allSkills[i];

                string buttonText;
                if (skillData.level < skillData.maxLevel)
                {
                    // 업그레이드 가능한 스킬
                    buttonText = $"{skillData.skillName}\nLv {skillData.level}/{skillData.maxLevel}";
                }
                else
                {
                    // 새로 해금 가능한 스킬
                    buttonText = $"{skillData.skillName}\n[스킬 해금]";
                }

                var buttonTextComponent = skillButtons[i].GetComponentInChildren<Text>();

                if (buttonTextComponent == null)
                {
                    Debug.LogError($"[LevelUpUI] 버튼 {i}에 Text 컴포넌트가 없습니다! Text 컴포넌트를 추가하세요.");
                }
                else
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
            Debug.LogError("[LevelUpUI] 선택된 스킬 데이터가 null입니다!");
            return;
        }

        // 스킬 업그레이드 또는 해금 처리
        GameManager.Instance.skillManager.UpgradeOrUnlockSkill(selectedSkill);

        // UI 닫기
        CloseUI();
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
