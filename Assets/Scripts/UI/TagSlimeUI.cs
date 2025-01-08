using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TagSlimeUI : UIBase
{
    private int tagSlimeIndex;

    private void OnTagSlime(int tagSlimeIndex, SlimeData slimeData)
    {
        if (tagSlimeIndex == GameManager.Instance.slimeManager.currentIndex)
        {
            Debug.Log("같은 슬라임 입니다!");
            return;
        }
        SaveSlimeSO();
        TagSlime(tagSlimeIndex);
        GameManager.Instance.expValue = slimeData.expValue;
        GameManager.Instance.currentHealthSystem.health = slimeData.health;
        GameManager.Instance.Level = slimeData.level;
    }

    public void OnTagDarkSlime()
    {
        OnTagSlime(0, GameManager.Instance.slimeManager.slimeDatas[0]);
    }

    public void OnTagElectricSlime()
    {
        OnTagSlime(1, GameManager.Instance.slimeManager.slimeDatas[1]);
    }

    public void OnTagFireSlime()
    {
        OnTagSlime(2, GameManager.Instance.slimeManager.slimeDatas[2]);
    }

    public void OnTagWaterSlime()
    {
        OnTagSlime(3, GameManager.Instance.slimeManager.slimeDatas[3]);
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
        return GameManager.Instance.slimeManager.slimeDatas[currentIndex];
    }

    private void UpdateSlimeData(SlimeData slimeData)
    {
        slimeData.level = GameManager.Instance.Level;
        slimeData.health = GameManager.Instance.currentHealthSystem.health;
        slimeData.expValue = GameManager.Instance.expValue;
    }
}