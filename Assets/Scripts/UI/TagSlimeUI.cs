using UnityEngine;
using UnityEngine.UI;

public class TagSlimeUI : MonoBehaviour
{
    public int tagSlimeIndex;

    void Start()
    {
        // SlimeManager ã��
        GameManager.Instance.slimeManager = FindObjectOfType<SlimeManager>();
    }

    void OnTagDarkSlime()
    {
        tagSlimeIndex = 0;
        if(tagSlimeIndex == GameManager.Instance.slimeManager.currentIndex)
        {
            Debug.Log("���� ������ �Դϴ�!");
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
            Debug.Log("���� ������ �Դϴ�!");
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
            Debug.Log("���� ������ �Դϴ�!");
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
            Debug.Log("���� ������ �Դϴ�!");
        }
        else
        {
            TagSlime(tagSlimeIndex);
        }
    }
    public void TagSlime(int index)
    {
        GameManager.Instance.slimeManager.release();

        // ���ο� ������ �ε��� ����
        GameManager.Instance.slimeManager.currentIndex = index;

        // ���ο� ������ ����
        GameManager.Instance.slimeManager.CreateSlime();
    }
}

