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
    }

    private void UpdateButton(Button button, SkillData skillData)
    {
        if (skillData != null)
        {
            Text buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = $"{skillData.skillName}\n레벨: {skillData.level}/{skillData.maxLevel}";
            button.gameObject.SetActive(true);
        }
        else
        {
            button.gameObject.SetActive(false);
        }
    }

    public void SelectSkill(int buttonIndex)
    {
        SkillData selectedSkill = buttonIndex switch
        {
            1 => skillData1,
            2 => skillData2,
            3 => skillData3,
            _ => null
        };

        if (selectedSkill == null)
        {
            Debug.LogError($"[LevelUpUI] 선택된 스킬 데이터가 유효하지 않습니다. 버튼 인덱스: {buttonIndex}");
            return;
        }

        if (selectedSkill.level < selectedSkill.maxLevel)
        {
            GameManager.Instance.skillManager.UpgradeSkill(selectedSkill.skillType, selectedSkill.element);
        }
        else
        {
            GameManager.Instance.skillManager.UnlockSkill(selectedSkill.skillType);
        }

        CloseUI();
    }
private void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
