using System.Collections.Generic;
using UnityEngine;

public class SkillPoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    private List<GameObject>[] pools;

    private void Awake()
    {
        // 초기화는 init()에서 수행
    }

    public void init()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        foreach (var obj in pools[index])
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        var newObject = Instantiate(prefabs[index]);
        pools[index].Add(newObject);
        return newObject;
    }

    public void ReturnToPool(GameObject obj, int index)
    {
        obj.SetActive(false);
        if (!pools[index].Contains(obj))
        {
            pools[index].Add(obj);
        }
    }
    public GameObject GetSkillPrefab(ElementType element, SkillManager.SkillType skillType)
    {
        string prefabName = $"{element}{skillType}";
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i].name == prefabName)
            {
                var skillPrefab = prefabs[i];
                // SkillDataComponent 검증
                SkillDataComponent skillDataComponent = skillPrefab.GetComponent<SkillDataComponent>();
                if (skillDataComponent == null || skillDataComponent.skillData == null)
                {
                    Debug.LogError($"[SkillPoolManager] {prefabName} 프리팹에 SkillDataComponent가 없거나 연결되지 않았습니다!");
                    return null;
                }
                return skillPrefab;
            }
        }
        Debug.LogWarning($"[SkillPoolManager] 스킬 프리팹 '{prefabName}'을 찾을 수 없습니다.");
        return null;
    }
    public void release()
    {
        // 필요에 따라 구현
    }
}
