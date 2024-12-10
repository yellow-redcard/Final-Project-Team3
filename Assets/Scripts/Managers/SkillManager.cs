using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour, IManager
{
    public enum Element { Dark, Electricity, Flame, Water }

    private Dictionary<Element, Dictionary<Skill.SkillType, int>> skillPrefabIndices;
    public PoolManager poolManager;
    private List<Skill> activeSkills = new List<Skill>(); // 활성화된 스킬 리스트
    private HashSet<Skill.SkillType> unlockedSkills = new HashSet<Skill.SkillType>() { Skill.SkillType.Single }; // 초기에는 단일기만 활성화
    private Element currentElement;

    public void init()
    {
        skillPrefabIndices = new Dictionary<Element, Dictionary<Skill.SkillType, int>>();
        LoadSkillPrefabs();
    }

    private void LoadSkillPrefabs()
    {
        foreach (Element element in System.Enum.GetValues(typeof(Element)))
        {
            Dictionary<Skill.SkillType, int> elementSkills = new Dictionary<Skill.SkillType, int>();

            for (int i = 0; i < poolManager.prefabs.Length; i++)
            {
                string prefabName = poolManager.prefabs[i].name;

                if (prefabName.Contains($"{element}Single"))
                    elementSkills[Skill.SkillType.Single] = i;
                else if (prefabName.Contains($"{element}Cone"))
                    elementSkills[Skill.SkillType.Cone] = i;
                else if (prefabName.Contains($"{element}Line"))
                    elementSkills[Skill.SkillType.Line] = i;
                else if (prefabName.Contains($"{element}Area"))
                    elementSkills[Skill.SkillType.Area] = i;
            }

            skillPrefabIndices[element] = elementSkills;
        }
    }

    public void SetCurrentElement(Element element)
    {
        currentElement = element;
    }

    public void FireSkill(Skill.SkillType skillType, Vector3 spawnPosition)
    {
        if (!unlockedSkills.Contains(skillType)) return; // 잠금된 스킬은 발사 불가

        if (skillPrefabIndices.ContainsKey(currentElement) && skillPrefabIndices[currentElement].ContainsKey(skillType))
        {
            int prefabIndex = skillPrefabIndices[currentElement][skillType];
            GameObject skillInstance = poolManager.Get(prefabIndex);
            skillInstance.transform.position = spawnPosition;
            skillInstance.SetActive(true);

            Skill skill = skillInstance.GetComponent<Skill>();
            if (skill != null)
            {
                activeSkills.Add(skill); // 활성화된 스킬 리스트에 추가
                skill.UseSkill();

                // 스킬이 종료되면 풀로 반환
                StartCoroutine(ReturnToPoolAfterUse(skillInstance, prefabIndex, skill.duration));
            }
        }
        else
        {
            Debug.LogError($"Skill not found for {currentElement} - {skillType}");
        }
    }
    private IEnumerator ReturnToPoolAfterUse(GameObject skillInstance, int prefabIndex, float duration)
    {
        yield return new WaitForSeconds(duration);
        poolManager.ReturnToPool(skillInstance, prefabIndex); // 풀로 반환
    }
    public void AdjustFireRate(float rate)
    {
        foreach (var skill in activeSkills)
        {
            skill.cooldown *= (1f - rate); // 발사 속도 증가
        }
        Debug.Log($"[SkillManager] 스킬 발사 속도 조정! Rate: {rate}");
    }

    public void UnlockSkill(Skill.SkillType skillType)
    {
        if (!unlockedSkills.Contains(skillType))
        {
            unlockedSkills.Add(skillType);
            Debug.Log($"스킬 {skillType} 해제 완료!");
        }
    }

    public void UpgradeSkill(Skill.SkillType skillType, float damageIncrease, float rangeIncrease, float cooldownDecrease)
    {
        foreach (var skill in activeSkills)
        {
            if (skill.skillType == skillType)
            {
                skill.damage += damageIncrease;
                skill.range += rangeIncrease;
                skill.cooldown = Mathf.Max(0.5f, skill.cooldown - cooldownDecrease); // 쿨타임은 최소 0.5초
                Debug.Log($"{skillType} 스킬 업그레이드! 데미지: +{damageIncrease}, 범위: +{rangeIncrease}, 쿨타임: -{cooldownDecrease}");
            }
        }
    }

    public void release()
    {
        foreach (var skill in activeSkills)
        {
            skill.Deactivate(); // 비활성화
        }
        activeSkills.Clear();
        Debug.Log("[SkillManager] 모든 스킬 정리 완료.");
    }
    public HashSet<Skill.SkillType> GetUnlockedSkills()
    {
        return new HashSet<Skill.SkillType>(unlockedSkills);
    }
}
