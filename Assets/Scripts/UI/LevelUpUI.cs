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

        levelUpDescription.text = "스킬이 강화됩니다."; // 설명 표시

        // 각 버튼 업데이트
        UpdateButton(skillButton1, skillData1, 1);
        UpdateButton(skillButton2, skillData2, 2);
        UpdateButton(skillButton3, skillData3, 3);
    }

    private void UpdateButton(Button button, SkillData skillData, int buttonIndex)
    {
        if (skillData != null)
        {
            Text buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = $"{skillData.skillName}\nLv.{skillData.level + 1} 스킬이 강화됩니다.";
            button.onClick.RemoveAllListeners(); // 기존 리스너 제거
            button.onClick.AddListener(() => SelectSkill(buttonIndex)); // 버튼 인덱스를 전달
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

        Debug.Log($"[LevelUpUI] 선택된 스킬: {selectedSkill.skillName}");
        GameManager.Instance.skillManager.UpgradeSkill(selectedSkill.skillType, selectedSkill.element);
        CloseUI();
    }

    private void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
