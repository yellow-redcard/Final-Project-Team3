using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TagSlimeUI : UIBase
{
    private int tagSlimeIndex;
    public List<SlimeData> slimeDatas;

    private void OnTagSlime(int tagSlimeIndex, SlimeData slimeData)
    {
        if (tagSlimeIndex == GameManager.Instance.slimeManager.currentIndex)
        {
            Debug.Log("같은 슬라임 입니다!");
            return;
        }
        SaveSlimeSO();
        TagSlime(tagSlimeIndex);
        foreach (SlimeData slime in slimeDatas)
        {
            Debug.Log("슬라임 태그 레벨: " + slime.level);
        }
        GameManager.Instance.expValue = slimeData.expValue;
        GameManager.Instance.hpValue = slimeData.hpValue;
        GameManager.Instance.Level = slimeData.level;
        Debug.Log("슬라임 태그 레벨 확인" + slimeData.level);
    }

    public void OnTagDarkSlime()
    {
        OnTagSlime(0, slimeDatas[0]);
    }

    public void OnTagElectricSlime()
    {
        OnTagSlime(1, slimeDatas[1]);
    }

    public void OnTagFireSlime()
    {
        OnTagSlime(2, slimeDatas[2]);
    }

    public void OnTagWaterSlime()
    {
        OnTagSlime(3, slimeDatas[3]);
    }
    private void TagSlime(int index)
    {
        Vector2 currentPosition = GameManager.Instance.slimeManager.currentSlime.transform.position;
        GameManager.Instance.slimeManager.slimeBodies[GameManager.Instance.slimeManager.currentIndex].SetActive(false);

        // 새로운 슬라임 인덱스 설정
        GameManager.Instance.slimeManager.currentIndex = index;

        // 새로운 슬라임 생성
        GameManager.Instance.slimeManager.ChangeSlime(currentPosition);

        // GameManager의 player 참조 업데이트
        GameManager.Instance.player = GameManager.Instance.slimeManager.currentSlime.transform;

        Time.timeScale = 1f;
        GameManager.Instance.uiManager.Hide<TagSlimeUI>();
    }
    private void SaveSlimeSO()
    {
        SlimeData currentSlimeData = GetCurrentSlimeData(GameManager.Instance.slimeManager.currentIndex);
        if (currentSlimeData != null)
        {
            UpdateSlimeData(currentSlimeData);
        }
    }

    private SlimeData GetCurrentSlimeData(int currentIndex)
    {
        return slimeDatas[currentIndex];
    }

    private void UpdateSlimeData(SlimeData slimeData)
    {
        slimeData.level = GameManager.Instance.Level;
        Debug.Log("슬라임 태그 레벨" + slimeData.level);
        slimeData.hpValue = GameManager.Instance.hpValue;
        slimeData.expValue = GameManager.Instance.expValue;
    }
}