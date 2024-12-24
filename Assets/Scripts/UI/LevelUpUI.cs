using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelUpUI : UIBase
{
    [SerializeField] private GameObject skillUp1; // SkillUp1 이미지
    [SerializeField] private GameObject skillUp2; // SkillUp2 이미지
    [SerializeField] private GameObject skillUp3; // SkillUp3 이미지
    [SerializeField] private Button closeButton;  // CloseButton 버튼

    private List<GameObject> skillButtons = new List<GameObject>();

    private void Start()
    {
        // 버튼 리스트 초기화
        skillButtons.Add(skillUp1);
        skillButtons.Add(skillUp2);
        skillButtons.Add(skillUp3);

        // CloseButton 클릭 이벤트 설정
        closeButton.onClick.AddListener(CloseUI);
    }

    public void ShowLevelUpUI()
    {
        // 스킬 업그레이드 옵션 가져오기
        var upgradeOptions = GameManager.Instance.skillManager.GetUpgradeOptions();

        // 각 스킬 업그레이드 이미지에 옵션 배정
        for (int i = 0; i < skillButtons.Count; i++)
        {
            if (i < upgradeOptions.Count)
            {
                // 버튼 활성화 및 텍스트 설정
                skillButtons[i].SetActive(true);

                // 이미지 하위에 텍스트 추가 (옵션 설명 표시)
                var buttonText = skillButtons[i].GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = $"{upgradeOptions[i].Item1}: {upgradeOptions[i].Item2}";
                }

                // 버튼 클릭 이벤트 설정
                var option = upgradeOptions[i]; // 클로저 문제 방지
                skillButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
                skillButtons[i].GetComponent<Button>().onClick.AddListener(() => SelectUpgrade(option.Item1, option.Item2));
            }
            else
            {
                // 남은 버튼 비활성화
                skillButtons[i].SetActive(false);
            }
        }

        gameObject.SetActive(true); // 레벨업 UI 활성화
        Time.timeScale = 0f;       // 게임 일시 정지
    }

    private void SelectUpgrade(SkillManager.SkillType skillType, string option)
    {
        GameManager.Instance.skillManager.UpgradeSkill(skillType, option);
        CloseUI();
    }

    public void CloseUI()
    {
        gameObject.SetActive(false); // UI 비활성화
        Time.timeScale = 1f;        // 게임 재개
    }
}
