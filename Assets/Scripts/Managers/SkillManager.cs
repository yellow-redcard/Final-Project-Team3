using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour, IManager
{
    public enum Element { Dark, Electricity, Flame, Water }

    private Dictionary<Element, Dictionary<Skill.SkillType, int>> skillPrefabIndices;
    public PoolManager poolManager;
    private List<Skill> activeSkills = new List<Skill>(); // Ȱ��ȭ�� ��ų ����Ʈ
    private HashSet<Skill.SkillType> unlockedSkills = new HashSet<Skill.SkillType>() { Skill.SkillType.Single }; // �ʱ⿡�� ���ϱ⸸ Ȱ��ȭ
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
        if (!unlockedSkills.Contains(skillType)) return; // ��ݵ� ��ų�� �߻� �Ұ�

        if (skillPrefabIndices.ContainsKey(currentElement) && skillPrefabIndices[currentElement].ContainsKey(skillType))
        {
            int prefabIndex = skillPrefabIndices[currentElement][skillType];
            GameObject skillInstance = poolManager.Get(prefabIndex);
            skillInstance.transform.position = spawnPosition;
            skillInstance.SetActive(true);

            Skill skill = skillInstance.GetComponent<Skill>();
            if (skill != null)
            {
                activeSkills.Add(skill); // Ȱ��ȭ�� ��ų ����Ʈ�� �߰�
                skill.UseSkill();

                // ��ų�� ����Ǹ� Ǯ�� ��ȯ
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
        poolManager.ReturnToPool(skillInstance, prefabIndex); // Ǯ�� ��ȯ
    }
    public void AdjustFireRate(float rate)
    {
        foreach (var skill in activeSkills)
        {
            skill.cooldown *= (1f - rate); // �߻� �ӵ� ����
        }
        Debug.Log($"[SkillManager] ��ų �߻� �ӵ� ����! Rate: {rate}");
    }

    public void UnlockSkill(Skill.SkillType skillType)
    {
        if (!unlockedSkills.Contains(skillType))
        {
            unlockedSkills.Add(skillType);
            Debug.Log($"��ų {skillType} ���� �Ϸ�!");
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
                skill.cooldown = Mathf.Max(0.5f, skill.cooldown - cooldownDecrease); // ��Ÿ���� �ּ� 0.5��
                Debug.Log($"{skillType} ��ų ���׷��̵�! ������: +{damageIncrease}, ����: +{rangeIncrease}, ��Ÿ��: -{cooldownDecrease}");
            }
        }
    }

    public void release()
    {
        foreach (var skill in activeSkills)
        {
            skill.Deactivate(); // ��Ȱ��ȭ
        }
        activeSkills.Clear();
        Debug.Log("[SkillManager] ��� ��ų ���� �Ϸ�.");
    }
    public HashSet<Skill.SkillType> GetUnlockedSkills()
    {
        return new HashSet<Skill.SkillType>(unlockedSkills);
    }
}
