using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour, IManager
{
    public enum Element { None, Dark, Electricity, Flame, Water }
    public enum SkillType { Single, Cone, Line, Area }

    private Dictionary<SkillType, HashSet<string>> skillUpgrades = new Dictionary<SkillType, HashSet<string>>()
    {
        { SkillType.Single, new HashSet<string> { "Cooldown", "Damage", "Projectile" } },
        { SkillType.Cone, new HashSet<string> { "Cooldown", "Damage", "Range" } },
        { SkillType.Line, new HashSet<string> { "Cooldown", "Damage", "Range" } },
        { SkillType.Area, new HashSet<string> { "Damage", "Range" } }
    };

    private Dictionary<Element, Dictionary<SkillType, int>> skillPrefabIndices;
    private HashSet<SkillType> unlockedSkills = new HashSet<SkillType> { SkillType.Single }; // 기본 스킬 포함
    private Dictionary<SkillType, float> skillCooldownTimers = new Dictionary<SkillType, float>(); // 쿨다운 타이머
    private Dictionary<SkillType, int> skillLevels = new Dictionary<SkillType, int>();
    private Element currentElement = Element.None;

    public SkillDatabase skillDatabase;
    private bool isFiring = false;

    // 초기화
    public void init()
    {
        skillPrefabIndices = new Dictionary<Element, Dictionary<SkillType, int>>();

        foreach (SkillType skillType in System.Enum.GetValues(typeof(SkillType)))
        {
            skillLevels[skillType] = 1; // 모든 스킬 초기 레벨 설정
            skillCooldownTimers[skillType] = 0f; // 초기 쿨다운
        }

        skillDatabase = FindObjectOfType<SkillDatabase>();
        if (skillDatabase == null)
        {
            Debug.LogError("[SkillManager] SkillDatabase가 설정되지 않았습니다.");
            return;
        }

        LoadSkillPrefabs(); // 스킬 프리팹 로드
        UnlockSkill(SkillType.Single); // 기본 스킬 해금

        StartCoroutine(AutoFireSkills());
    }


    // 스킬 프리팹 로드
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
    private IEnumerator AutoFireSkills()
    {
        // MonsterPoolManager 인스턴스 가져오기
        MonsterPoolManager monsterPoolManager = FindObjectOfType<MonsterPoolManager>();
        if (monsterPoolManager == null)
        {
            Debug.LogError("[SkillManager] MonsterPoolManager를 찾을 수 없습니다.");
            yield break;
        }

        while (true)
        {
            // MonsterPoolManager에서 활성화된 몬스터 리스트 가져오기
            List<Transform> enemies = monsterPoolManager.GetActiveMonsters();

            foreach (SkillType skillType in unlockedSkills)
            {
                if (skillCooldownTimers[skillType] <= 0)
                {
                    FireSkill(skillType, GameManager.Instance.player.position, enemies);
                    ResetSkillCooldown(skillType);
                }
            }

            // 쿨다운 타이머 업데이트
            UpdateCooldownTimers();
            yield return null;
        }
    }
    private void UpdateCooldownTimers()
    {
        foreach (SkillType skillType in unlockedSkills)
        {
            if (skillCooldownTimers[skillType] > 0)
            {
                skillCooldownTimers[skillType] -= Time.deltaTime;
            }
        }
    }


    // 현재 사용 속성 설정
    public void SetCurrentElement(Element element)
    {
        currentElement = element;
    }
    private void ResetSkillCooldown(SkillType skillType)
    {
        SkillData skillData = skillDatabase.GetSkillData(skillType, currentElement);
        if (skillData != null)
        {
            int currentLevel = skillLevels[skillType];
            skillCooldownTimers[skillType] = skillData.levelUpStats[currentLevel - 1].cooldown;
        }
    }
    // 스킬 사용

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

    // 단일 대상 선택
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

    // 스킬 생성
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
        else
        {
            Debug.LogError($"[SpawnSkill] {currentElement} 속성 또는 {skillType} 스킬의 프리팹이 존재하지 않습니다.");
        }
    }

    private IEnumerator ReturnToPoolAfterUse(GameObject skillInstance, int prefabIndex, float duration)
    {
        yield return new WaitForSeconds(duration);
        skillInstance.SetActive(false);
        GameManager.Instance.skillPool.ReturnToPool(skillInstance, prefabIndex);
    }

// 스킬 해금
public void UnlockSkill(SkillType skillType)
    {
        if (!unlockedSkills.Contains(skillType))
        {
            unlockedSkills.Add(skillType);
            skillLevels[skillType] = 1;
            Debug.Log($"스킬 {skillType} 해금!");
        }
    }

    // 스킬 업그레이드
    public void UpgradeSkill(SkillManager.SkillType skillType, SkillManager.Element element)
    {
        SkillData skillData = skillDatabase.GetSkillData(skillType, element);
        if (skillData == null)
        {
            Debug.LogError($"[SkillManager] {element} 속성의 {skillType} 스킬 데이터를 찾을 수 없습니다.");
            return;
        }

        int currentLevel = skillLevels[skillType];
        if (currentLevel >= skillData.maxLevel)
        {
            Debug.LogWarning($"[SkillManager] {skillData.skillName}은(는) 이미 최대 레벨입니다.");
            return;
        }

        // 레벨 업
        currentLevel++;
        skillLevels[skillType] = currentLevel;

        // 현재 레벨 데이터 가져오기
        SkillData.LevelUpStats stats = skillData.levelUpStats[currentLevel - 1]; // 배열은 0-based

        Debug.Log($"[SkillManager] {skillData.skillName} 업그레이드 완료! 레벨: {currentLevel}, 데미지: {stats.damage}, 쿨다운: {stats.cooldown}, 사거리: {stats.range}");
    }

    // 레벨업 옵션 생성
    public List<SkillData> GetLevelUpOptions()
    {
        List<SkillData> options = new List<SkillData>();

        // 해금된 스킬 업그레이드 선택지 추가
        foreach (SkillType skillType in unlockedSkills)
        {
            SkillData skillData = skillDatabase.GetSkillData(skillType, currentElement);
            if (skillData != null && skillData.level < skillData.maxLevel)
            {
                options.Add(skillData);
            }
        }

        // 아직 해금되지 않은 스킬 선택지 추가
        foreach (SkillType skillType in System.Enum.GetValues(typeof(SkillType)))
        {
            if (!unlockedSkills.Contains(skillType))
            {
                SkillData skillData = skillDatabase.GetSkillData(skillType, currentElement);
                if (skillData != null)
                {
                    options.Add(skillData);
                }
            }
        }

        return options.OrderBy(_ => Random.value).Take(3).ToList(); // 무작위로 최대 3개 선택
    }

    // 해금된 스킬 반환
    public HashSet<SkillType> GetUnlockedSkills()
    {
        return new HashSet<SkillType>(unlockedSkills);
    }

    public void release() { }
}
