using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour, IManager
{
    public List<GameObject> slimePrefabs;
    public GameObject currentSlime;
    public int currentIndex;

    // 슬라임 이름에 따라 속성을 매핑합니다.
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

        // 생성된 슬라임 이름으로 속성을 설정
        if (currentSlime != null)
        {
            SetElementBySlime(currentSlime.name);
        }
    }

    private void SetElementBySlime(string slimeName)
    {
        // 이름에서 "(Clone)" 제거
        string cleanName = slimeName.Replace("(Clone)", "").Trim();

        if (slimeToElementMap.TryGetValue(cleanName, out SkillManager.Element element))
        {
            // SkillManager의 현재 속성을 설정
            GameManager.Instance.skillManager.SetCurrentElement(element);
            Debug.Log($"슬라임 '{cleanName}'에 따라 속성을 '{element}'로 설정했습니다.");
        }
        else
        {
            Debug.LogWarning($"'{cleanName}'에 해당하는 속성을 찾을 수 없습니다.");
        }
    }
}
