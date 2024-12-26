using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour, IManager
{
    public enum Element { Dark, Electricity, Flame, Water }
    public enum SkillType { Single, Cone, Line, Area }

    private Dictionary<SkillType, HashSet<string>> skillUpgrades = new Dictionary<SkillType, HashSet<string>>()
    {
        { SkillType.Single, new HashSet<string> { "Cooldown", "Damage", "Projectile" } },
        { SkillType.Cone, new HashSet<string> { "Cooldown", "Damage", "Range" } },
        { SkillType.Line, new HashSet<string> { "Cooldown", "Damage", "Range" } },
        { SkillType.Area, new HashSet<string> { "Damage", "Range" } }
    };

    private Dictionary<Element, Dictionary<SkillType, int>> skillPrefabIndices;
    private HashSet<SkillType> unlockedSkills = new HashSet<SkillType> { SkillType.Single };
    private Dictionary<SkillType, int> skillLevels = new Dictionary<SkillType, int>();
    private List<Skill> activeSkills = new List<Skill>();
    private Element currentElement;

    public SkillDatabase skillDatabase;
    public void release()
    {

    }
    public void init()
    {
        skillPrefabIndices = new Dictionary<Element, Dictionary<SkillType, int>>();

        foreach (SkillType skillType in System.Enum.GetValues(typeof(SkillType)))
        {
            skillLevels[skillType] = 1;
        }

        skillDatabase = FindObjectOfType<SkillDatabase>();
        if (skillDatabase == null)
        {
            Debug.LogError("[SkillManager] SkillDatabase가 설정되지 않았습니다.");
            return;
        }

        LoadSkillPrefabs();
        UnlockSkill(SkillType.Single);
    }

    private void LoadSkillPrefabs()
    {
        foreach (Element element in System.Enum.GetValues(typeof(Element)))
        {
            Dictionary<SkillType, int> elementSkills = new Dictionary<SkillType, int>();

            for (int i = 0; i < GameManager.Instance.skillPool.prefabs.Length; i++)
            {
                string prefabName = GameManager.Instance.skillPool.prefabs[i].name;

                if (prefabName.Contains($"{element}Single"))
                    elementSkills[SkillType.Single] = i;
                else if (prefabName.Contains($"{element}Cone"))
                    elementSkills[SkillType.Cone] = i;
                else if (prefabName.Contains($"{element}Line"))
                    elementSkills[SkillType.Line] = i;
                else if (prefabName.Contains($"{element}Area"))
                    elementSkills[SkillType.Area] = i;
            }

            skillPrefabIndices[element] = elementSkills;
        }
    }

    public void SetCurrentElement(Element element)
    {
        currentElement = element;
    }

    public void FireSkill(SkillType skillType, Vector3 playerPosition, List<Transform> enemies)
    {
        if (!unlockedSkills.Contains(skillType)) return;

        SkillData skillData = skillDatabase.GetSkillData(skillType, currentElement);
        if (skillData == null)
        {
            Debug.LogWarning($"[SkillManager] {currentElement} {skillType} 스킬 데이터가 없습니다.");
            return;
        }

        if (skillType == SkillType.Area)
        {
            SpawnSkill(skillType, playerPosition, skillData);
        }
        else
        {
            Transform targetEnemy = GetSingleTarget(enemies);
            if (targetEnemy != null)
            {
                SpawnSkill(skillType, targetEnemy.position, skillData);
            }
        }
    }

    private Transform GetSingleTarget(List<Transform> enemies)
    {
        if (enemies == null || enemies.Count == 0) return null;

        Transform closestEnemy = enemies[0];
        float closestDistance = Vector3.Distance(GameManager.Instance.player.position, closestEnemy.position);

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(GameManager.Instance.player.position, enemy.position);
            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }

        return closestEnemy;
    }

    private void SpawnSkill(SkillType skillType, Vector3 position, SkillData skillData)
    {
        if (skillPrefabIndices.ContainsKey(currentElement) && skillPrefabIndices[currentElement].ContainsKey(skillType))
        {
            int prefabIndex = skillPrefabIndices[currentElement][skillType];
            GameObject skillInstance = GameManager.Instance.skillPool.Get(prefabIndex);
            skillInstance.transform.position = position;
            skillInstance.SetActive(true);

            Skill skill = skillInstance.GetComponent<Skill>();
            if (skill != null)
            {
                skill.baseDamage = skillData.baseDamage;
                skill.cooldown = skillData.cooldown;
                skill.baseRange = skillData.baseRange;
                skill.duration = skillData.duration;
                skill.projectileCount = skillData.projectileCount;

                skill.UseSkill();
                StartCoroutine(ReturnToPoolAfterUse(skillInstance, prefabIndex, skill.duration));
            }
        }
    }

    private IEnumerator ReturnToPoolAfterUse(GameObject skillInstance, int prefabIndex, float duration)
    {
        yield return new WaitForSeconds(duration);
        GameManager.Instance.skillPool.ReturnToPool(skillInstance, prefabIndex);
    }

    public void UnlockSkill(SkillType skillType)
    {
        if (!unlockedSkills.Contains(skillType))
        {
            unlockedSkills.Add(skillType);
            skillLevels[skillType] = 1;
            Debug.Log($"스킬 {skillType} 해금!");
        }
    }

    public void UpgradeSkill(SkillType skillType, Element element)
    {
        SkillData skillData = skillDatabase.GetSkillData(skillType, element);
        if (skillData == null)
        {
            Debug.LogError($"[SkillManager] {element} 속성의 {skillType} 스킬 데이터를 찾을 수 없습니다.");
            return;
        }

        if (skillData.upgradeModifiers.ContainsKey("Damage"))
        {
            skillData.baseDamage += skillData.upgradeModifiers["Damage"];
        }

        if (skillData.upgradeModifiers.ContainsKey("Cooldown"))
        {
            skillData.cooldown += skillData.upgradeModifiers["Cooldown"];
        }

        Debug.Log($"스킬 {skillData.skillName} 업그레이드 완료!");
    }

    public List<SkillData> GetLevelUpOptions()
    {
        List<SkillData> options = new List<SkillData>();

        // 이미 해금된 스킬 추가
        foreach (var skill in unlockedSkills)
        {
            SkillData skillData = skillDatabase.GetSkillData(skill, currentElement);
            if (skillData != null)
            {
                options.Add(skillData); // 리스트에 추가
            }
        }

        // 신규 스킬 추가 (랜덤 확률)
        foreach (SkillType skill in System.Enum.GetValues(typeof(SkillType)))
        {
            if (!unlockedSkills.Contains(skill) && Random.value < 0.2f)
            {
                SkillData skillData = skillDatabase.GetSkillData(skill, currentElement);
                if (skillData != null)
                {
                    options.Add(skillData);
                }
            }
        }

        // 반환 전 디버그
        Debug.Log("[SkillManager] 레벨업 옵션 순서:");
        foreach (var option in options)
        {
            Debug.Log($"[SkillManager] 스킬 이름: {option.skillName}");
        }

        return options.GetRange(0, Mathf.Min(3, options.Count)); // 최대 3개 반환
    }

    public void UpgradeOrUnlockSkill(SkillType skillType, Element element, bool isNewSkill)
    {
        if (isNewSkill)
        {
            UnlockSkill(skillType);
        }
        else
        {
            UpgradeSkill(skillType, element);
        }
    }

    public HashSet<SkillType> GetUnlockedSkills()
    {
        return new HashSet<SkillType>(unlockedSkills);
    }
}
