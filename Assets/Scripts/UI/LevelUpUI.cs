using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : UIBase
{
    [SerializeField] private Text titleText;
    [SerializeField] private Button skillButton1;
    [SerializeField] private Button skillButton2;
    [SerializeField] private Button skillButton3;

    private SkillData skillData1;
    private SkillData skillData2;
    private SkillData skillData3;
    public void ConfigureButtons(SkillData data1, SkillData data2, SkillData data3)
    {
        skillData1 = data1;
        skillData2 = data2;
        skillData3 = data3;

        UpdateButton(skillButton1, skillData1);
        UpdateButton(skillButton2, skillData2);
        UpdateButton(skillButton3, skillData3);

        Debug.Log($"[LevelUpUI] ConfigureButtons 호출 완료");
        Debug.Log($"[LevelUpUI] skillData1: {skillData1?.skillName ?? "null"}");
        Debug.Log($"[LevelUpUI] skillData2: {skillData2?.skillName ?? "null"}");
        Debug.Log($"[LevelUpUI] skillData3: {skillData3?.skillName ?? "null"}");

    }

    private void UpdateButton(Button button, SkillData skillData)
    {
        if (skillData != null)
        {
            Text buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = $"{skillData.skillName}\n{skillData.description}";

            button.gameObject.SetActive(true);
        }
        else
        {
            button.gameObject.SetActive(false);
        }
    }

    public void SelectSkill(int buttonIndex)
    {
        SkillData selectedSkillData = buttonIndex switch
        {
            1 => skillData1,
            2 => skillData2,
            3 => skillData3,
            _ => null
        };

        if (selectedSkillData == null)
        {
            Debug.LogError($"[LevelUpUI] 선택된 스킬 데이터가 유효하지 않습니다. 버튼 인덱스: {buttonIndex}");
            Debug.Log($"[LevelUpUI] skillData1: {skillData1?.skillName ?? "null"}");
            Debug.Log($"[LevelUpUI] skillData2: {skillData2?.skillName ?? "null"}");
            Debug.Log($"[LevelUpUI] skillData3: {skillData3?.skillName ?? "null"}");
            return;
        }

        Debug.Log($"[LevelUpUI] 선택된 스킬: {selectedSkillData.skillName}");

        // SkillManager로 데이터 전달
        GameManager.Instance.skillManager.UpgradeOrUnlockSkill(
            selectedSkillData.skillType,
            selectedSkillData.element,
            selectedSkillData.level == 1 // 신규 스킬 여부
        );

        CloseUI();
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
