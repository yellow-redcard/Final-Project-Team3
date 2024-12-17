using UnityEngine;
using UnityEngine.UI;

public class TagSlimeUI : MonoBehaviour
{
    public int tagSlimeIndex;

    void Start()
    {
        // SlimeManager 찾기
        GameManager.Instance.slimeManager = FindObjectOfType<SlimeManager>();
    }

    void OnTagDarkSlime()
    {
        tagSlimeIndex = 0;
        if(tagSlimeIndex == GameManager.Instance.slimeManager.currentIndex)
        {
            Debug.Log("같은 슬라임 입니다!");
        }
        else
        {
            TagSlime(tagSlimeIndex);
        }
    }
    void OnTagElectricSlime()
    {
        tagSlimeIndex = 1;
        if (tagSlimeIndex == GameManager.Instance.slimeManager.currentIndex)
        {
            Debug.Log("같은 슬라임 입니다!");
        }
        else
        {
            TagSlime(tagSlimeIndex);
        }
    }
    void OnTagFireSlime()
    {
        tagSlimeIndex = 2;
        if (tagSlimeIndex == GameManager.Instance.slimeManager.currentIndex)
        {
            Debug.Log("같은 슬라임 입니다!");
        }
        else
        {
            TagSlime(tagSlimeIndex);
        }
    }
    void OnTagWaterSlime()
    {
        tagSlimeIndex = 3;
        if (tagSlimeIndex == GameManager.Instance.slimeManager.currentIndex)
        {
            Debug.Log("같은 슬라임 입니다!");
        }
        else
        {
            TagSlime(tagSlimeIndex);
        }
    }
    public void TagSlime(int index)
    {
        GameManager.Instance.slimeManager.release();

        // 새로운 슬라임 인덱스 설정
        GameManager.Instance.slimeManager.currentIndex = index;

        // 새로운 슬라임 생성
        GameManager.Instance.slimeManager.CreateSlime();
    }
}

