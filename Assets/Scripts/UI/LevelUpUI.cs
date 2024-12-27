using UnityEngine;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : UIBase
{
    [SerializeField] private Button skillButton1;
    [SerializeField] private Button skillButton2;
    [SerializeField] private Button skillButton3;
    [SerializeField] private Text levelUpDescription; // 설명 텍스트 추가

    private SkillData skillData1, skillData2, skillData3;

    public void ConfigureButtons(SkillData data1, SkillData data2, SkillData data3)
    {
        skillData1 = data1;
        skillData2 = data2;
        skillData3 = data3;

        // 데이터가 null인지 확인
        Debug.Log($"[ConfigureButtons] Data1: {data1?.skillName}, Data2: {data2?.skillName}, Data3: {data3?.skillName}");

        UpdateButton(skillButton1, skillData1, 1);
        UpdateButton(skillButton2, skillData2, 2);
        UpdateButton(skillButton3, skillData3, 3);
    }
    private void UpdateButton(Button button, SkillData skillData, int buttonIndex)
    {
        if (skillData != null)
        {
            Text buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = $"{skillData.skillName} (Lv {skillData.level + 1})";
            Debug.Log($"[UpdateButton] Button {buttonIndex} Text: {buttonText.text}");
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => SelectSkill(buttonIndex));
            button.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log($"[UpdateButton] Button {buttonIndex} is disabled.");
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

        Debug.Log($"[LevelUpUI] 선택된 스킬: {selectedSkill.skillName}");

        // GameManager 또는 SkillManager가 null인지 확인
        if (GameManager.Instance == null || GameManager.Instance.skillManager == null)
        {
            Debug.LogError("[LevelUpUI] GameManager 또는 SkillManager가 설정되지 않았습니다.");
            return;
        }

        GameManager.Instance.skillManager.UpgradeSkill(selectedSkill.skillType, selectedSkill.element);
        CloseUI();
    }
    private void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
