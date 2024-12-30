using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : UIBase
{
    [SerializeField] private Button skillButton1;
    [SerializeField] private Button skillButton2;
    [SerializeField] private Button skillButton3;

    private SkillData skillData1, skillData2, skillData3;

    public void ConfigureButtons(SkillData data1, SkillData data2, SkillData data3)
    {
        skillData1 = data1;
        skillData2 = data2;
        skillData3 = data3;

        UpdateButton(skillButton1, skillData1);
        UpdateButton(skillButton2, skillData2);
        UpdateButton(skillButton3, skillData3);

        gameObject.SetActive(true); // UI 활성화
    }

    private void UpdateButton(Button button, SkillData skillData)
    {
        if (skillData != null)
        {
            Text buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = $"{skillData.skillName}\n레벨: {skillData.level}/{skillData.maxLevel}\n{skillData.upgradeDescription}";

            button.gameObject.SetActive(true);
            button.onClick.RemoveAllListeners(); // 기존 이벤트 제거
            button.onClick.AddListener(() => OnSkillSelected(skillData)); // SkillData를 전달하는 이벤트 추가
        }
        else
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
        }
    }

    private void OnSkillSelected(SkillData skillData)
    {
        if (skillData == null)
        {
            Debug.LogError("[LevelUpUI] 선택된 스킬 데이터가 유효하지 않습니다.");
            return;
        }

        Debug.Log($"[LevelUpUI] 선택된 스킬: {skillData.skillName}");

        // SkillManager를 통해 스킬 강화
        GameManager.Instance.skillManager.UpgradeSkill(skillData.skillType, skillData.element);

        CloseUI();
    }

    private void CloseUI()
    {
        gameObject.SetActive(false); // UI 비활성화
    }
}
