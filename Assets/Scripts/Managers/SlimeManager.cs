using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour, IManager
{
    public List<GameObject> slimePrefabs;
    public GameObject currentSlime;
    public int currentIndex;

    // ������ �̸��� ���� �Ӽ��� �����մϴ�.
    private Dictionary<string, SkillManager.Element> slimeToElementMap = new Dictionary<string, SkillManager.Element>
    {
        { "DarkSlime", SkillManager.Element.Dark },
        { "FireSlime", SkillManager.Element.Flame },
        { "WaterSlime", SkillManager.Element.Water },
        { "ElectricSlime", SkillManager.Element.Electricity }
    };

    public void init()
    {
        currentIndex = Random.Range(0, slimePrefabs.Count);
        if (slimePrefabs.Count > 0)
        {
            CreateSlime();
        }
    }

    public void release()
    {
        Destroy(currentSlime);
    }

    public void CreateSlime()
    {
        if (currentSlime != null)
        {
            Destroy(currentSlime);
        }

        currentSlime = Instantiate(slimePrefabs[currentIndex], transform.position, Quaternion.identity);

        // ������ ������ �̸����� �Ӽ��� ����
        if (currentSlime != null)
        {
            SetElementBySlime(currentSlime.name);
        }
    }

    private void SetElementBySlime(string slimeName)
    {
        // �̸����� "(Clone)" ����
        string cleanName = slimeName.Replace("(Clone)", "").Trim();

        if (slimeToElementMap.TryGetValue(cleanName, out SkillManager.Element element))
        {
            // SkillManager�� ���� �Ӽ��� ����
            GameManager.Instance.skillManager.SetCurrentElement(element);
            Debug.Log($"������ '{cleanName}'�� ���� �Ӽ��� '{element}'�� �����߽��ϴ�.");
        }
        else
        {
            Debug.LogWarning($"'{cleanName}'�� �ش��ϴ� �Ӽ��� ã�� �� �����ϴ�.");
        }
    }
}
