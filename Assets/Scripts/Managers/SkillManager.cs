using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour, IManager
{
    public enum Element { Dark, Electricity, Flame, Water }
    public enum SkillType { Single, Cone, Line, Area }

    private Dictionary<Element, Dictionary<SkillType, int>> skillPrefabIndices;
    private HashSet<SkillType> unlockedSkills = new HashSet<SkillType> { SkillType.Single };
    private Dictionary<SkillType, int> skillLevels = new Dictionary<SkillType, int>();
    private List<Skill> activeSkills = new List<Skill>(); // 활성화된 스킬 목록
    private Element currentElement;

    private SkillDatabase skillDatabase;
    /// <summary>
    /// 초기화 메서드 (Manager Init 호출)
    /// </summary>
    public void init()
    {
        skillPrefabIndices = new Dictionary<Element, Dictionary<SkillType, int>>();

        // 모든 스킬 레벨 초기화
        foreach (SkillType skillType in System.Enum.GetValues(typeof(SkillType)))
        {
            skillLevels[skillType] = 1;
        }

        // SkillDatabase 참조
        skillDatabase = FindObjectOfType<SkillDatabase>();
        if (skillDatabase == null)
        {
            Debug.LogError("[SkillManager] SkillDatabase가 설정되지 않았습니다.");
            return;
        }

        LoadSkillPrefabs();
        UnlockSkill(SkillType.Single); // 기본 스킬 해금
    }

    /// <summary>
    /// 스킬 프리팹 로드 (Element 및 SkillType 별로 분류)
    /// </summary>
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

    /// <summary>
    /// 현재 속성 설정
    /// </summary>
    public void SetCurrentElement(Element element)
    {
        currentElement = element;
    }

    /// <summary>
    /// 스킬 발사
    /// </summary>
    public void FireSkill(SkillType skillType, Vector3 playerPosition, List<Transform> enemies)
    {
        if (!unlockedSkills.Contains(skillType)) return;

        SkillData skillData = skillDatabase.GetSkillData(skillType, currentElement); // SkillData 사용
        if (skillData == null)
        {
            Debug.LogWarning($"[SkillManager] {currentElement} {skillType} 스킬 데이터가 없습니다.");
            return;
        }

        if (skillType == SkillType.Area) // 장판 스킬은 플레이어 위치에 생성
        {
            SpawnSkill(skillType, playerPosition, skillData);
        }
        else // 단일기, 원뿔, 일직선은 한 마리의 몬스터를 타겟팅
        {
            Transform targetEnemy = GetSingleTarget(enemies);
            if (targetEnemy != null)
            {
                SpawnSkill(skillType, targetEnemy.position, skillData);
            }
        }
    }

    /// <summary>
    /// 가장 가까운 적 타겟팅
    /// </summary>
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

    /// <summary>
    /// 스킬 생성 및 사용
    /// </summary>
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
                // SkillData 기반으로 스킬 초기화
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

    /// <summary>
    /// 스킬 사용 후 풀에 반환
    /// </summary>
    private IEnumerator ReturnToPoolAfterUse(GameObject skillInstance, int prefabIndex, float duration)
    {
        yield return new WaitForSeconds(duration);
        GameManager.Instance.skillPool.ReturnToPool(skillInstance, prefabIndex);
    }

    /// <summary>
    /// 스킬 잠금 해제
    /// </summary>
    public void UnlockSkill(SkillType skillType)
    {
        if (!unlockedSkills.Contains(skillType))
        {
            unlockedSkills.Add(skillType);
            skillLevels[skillType] = 1;
            Debug.Log($"스킬 {skillType} 해금!");
        }
    }

    /// <summary>
    /// 스킬 업그레이드
    /// </summary>
    public void UpgradeSkill(SkillType skillType, string option)
    {
        if (option == "Unlock")
        {
            UnlockSkill(skillType);
        }
        else
        {
            skillLevels[skillType]++;
            Debug.Log($"스킬 {skillType} {option} 업그레이드! 현재 레벨: {skillLevels[skillType]}");
        }
    }

    /// <summary>
    /// 현재 스킬 레벨 반환
    /// </summary>
    public int GetSkillLevel(SkillType skillType)
    {
        return skillLevels.ContainsKey(skillType) ? skillLevels[skillType] : 0;
    }

    /// <summary>
    /// 잠금 해제된 스킬 목록 반환
    /// </summary>
    public HashSet<SkillType> GetUnlockedSkills()
    {
        return new HashSet<SkillType>(unlockedSkills);
    }

    /// <summary>
    /// 발사 속도 조정
    /// </summary>
    public void AdjustFireRate(float rate)
    {
        foreach (var skill in activeSkills)
        {
            skill.cooldown = Mathf.Max(0.1f, skill.cooldown * (1f - rate)); // 최소 쿨타임 0.1초
        }

        Debug.Log($"[SkillManager] 모든 스킬의 발사 속도가 {rate * 100}% 만큼 증가했습니다.");
    }

    /// <summary>
    /// Manager Release
    /// </summary>
    public void release()
    {
        activeSkills.Clear();
    }

    /// <summary>
    /// 레벨업 업그레이드 선택지 반환
    /// </summary>
    public List<(SkillType, string)> GetUpgradeOptions()
    {
        List<(SkillType, string)> options = new List<(SkillType, string)>();

        // 잠금 해제된 스킬 업그레이드 옵션 추가
        foreach (var skill in unlockedSkills)
        {
            options.Add((skill, "Cooldown"));
            options.Add((skill, "Damage"));
            if (skill == SkillType.Single) options.Add((skill, "Projectile"));
            if (skill != SkillType.Single) options.Add((skill, "Range"));
        }

        // 잠금 해제되지 않은 광역 스킬 추가 (확률적으로)
        var lockedSkills = new List<SkillType> { SkillType.Cone, SkillType.Line, SkillType.Area }
            .FindAll(skill => !unlockedSkills.Contains(skill));

        if (lockedSkills.Count > 0)
        {
            SkillType randomSkill = lockedSkills[Random.Range(0, lockedSkills.Count)];
            options.Add((randomSkill, "Unlock"));
        }

        // 최대 3개의 선택지 제한
        return options.GetRange(0, Mathf.Min(3, options.Count));
    }
}
