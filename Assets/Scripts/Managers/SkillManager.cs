using System.Collections;
using System.Collections.Generic;
using UnityEngine;using System.Linq; // For Random OrderBy
using static ElementSystem;
public class SkillManager : MonoBehaviour, IManager
{
    
    public enum SkillType { Single, Cone, Line, Area }

    private Dictionary<SkillType, HashSet<string>> skillUpgrades = new Dictionary<SkillType, HashSet<string>>()
    {
        { SkillType.Single, new HashSet<string> { "Cooldown", "Damage", "Projectile" } },
        { SkillType.Cone, new HashSet<string> { "Cooldown", "Damage", "Range" } },
        { SkillType.Line, new HashSet<string> { "Cooldown", "Damage", "Range" } },
        { SkillType.Area, new HashSet<string> { "Damage", "Range" } }
    };

    private Dictionary<ElementType, Dictionary<SkillType, int>> skillPrefabIndices;
    private HashSet<SkillType> unlockedSkills = new HashSet<SkillType> { SkillType.Single }; // 기본 스킬 포함
    private Dictionary<SkillType, float> skillCooldownTimers = new Dictionary<SkillType, float>(); // 쿨다운 타이머
    private Dictionary<SkillType, int> skillLevels = new Dictionary<SkillType, int>();
    public ElementType currentElement = ElementType.None;
    public List<GameObject> skillPrefabs;

    public SkillDatabase skillDatabase;
    private bool isFiring = false;

    public void init()
    {
        skillPrefabIndices = new Dictionary<ElementType, Dictionary<SkillType, int>>();

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

        Debug.Log("[SkillManager] SkillDatabase 초기화 성공");
        StartCoroutine(AutoFireSkills());
        LoadSkillPrefabs();
        UnlockSkill(SkillType.Single); // 기본 스킬 해금
    }
    private void LoadSkillPrefabs()
    {
        foreach (ElementType element in System.Enum.GetValues(typeof(ElementType)))
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

    public IEnumerator AutoFireSkills()
    {
        MonsterPoolManager monsterPoolManager = FindObjectOfType<MonsterPoolManager>();
        if (monsterPoolManager == null)
        {
            Debug.LogError("[SkillManager] MonsterPoolManager를 찾을 수 없습니다.");
            yield break;
        }

        while (true)
        {
            List<Transform> enemies = monsterPoolManager.GetActiveMonsters();

            //삭제된 오브젝트 제거
            enemies.RemoveAll(enemy => enemy == null || !enemy.gameObject.activeSelf);

            foreach (SkillType skillType in unlockedSkills)
            {
                if (skillCooldownTimers[skillType] <= 0)
                {
                    FireSkill(skillType, GameManager.Instance.player.position, enemies);
                    ResetSkillCooldown(skillType);
                }
            }

            //쿨다운 타이머 업데이트
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

    public void SetCurrentElement(ElementType element)
    {
        currentElement = element; // 현재 속성 업데이트
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

    public void FireSkill(SkillType skillType, Vector3 playerPosition, List<Transform> enemies)
    {
        if (!unlockedSkills.Contains(skillType))
        {
            Debug.LogWarning($"[SkillManager] {skillType} 스킬이 해금되지 않았습니다.");
            return;
        }

        SkillData skillData = skillDatabase.GetSkillData(skillType, currentElement);
        if (skillData == null)
        {
            Debug.LogError($"[SkillManager] {currentElement} {skillType} 스킬 데이터가 없습니다!");
            return;
        }
        if (enemies == null || enemies.Count == 0)
        {
            Debug.LogWarning($"[SkillManager] {skillType} 스킬 대상 적 없음");
            return;
        }

        Debug.Log($"[SkillManager] {currentElement} {skillType} 스킬 발동 성공");
        SpawnSkill(skillType, playerPosition, skillData);
    
    Debug.Log($"[SkillManager] {currentElement} {skillType} 스킬 데이터 로드 완료: {skillData.skillName}");

        // 삭제된 적 체크 및 제거
        enemies.RemoveAll(enemy => enemy == null || !enemy.gameObject.activeSelf);

        if (skillType == SkillType.Area)
        {
            SpawnSkill(skillType, playerPosition, skillData);
        }
        else
        {
            Transform targetEnemy = GetSingleTarget(enemies);
            if (targetEnemy != null)
            {
                Debug.Log($"[SkillManager] {skillType} 스킬 대상: {targetEnemy.name}");
                SpawnSkill(skillType, targetEnemy.position, skillData);
            }
            else
            {
                Debug.LogWarning($"[SkillManager] {skillType} 스킬 대상 적 없음");
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

            if (skillInstance != null)
            {
                skillInstance.transform.position = position;
                skillInstance.SetActive(true);
                Debug.Log($"[SkillManager] {skillType} 스킬 활성화 성공: {skillInstance.name}");

                Skill skill = skillInstance.GetComponent<Skill>();
                if (skill != null)
                {
                    skill.baseDamage = skillData.baseDamage;
                    skill.cooldown = skillData.cooldown;
                    skill.baseRange = skillData.baseRange;
                    skill.duration = skillData.duration;
                    skill.projectileCount = skillData.projectileCount;
                    skill.UseSkill();
                }
                else
                {
                    Debug.LogError($"[SkillManager] {skillInstance.name}에 Skill 컴포넌트가 없습니다!");
                }
            }
            else
            {
                Debug.LogError($"[SkillManager] 스킬 프리팹 인스턴스를 가져오지 못했습니다.");
            }
        }
        else
        {
            Debug.LogError($"[SkillManager] {currentElement} 속성의 {skillType} 스킬 프리팹이 없습니다.");
        }
    }

    private IEnumerator ReturnToPoolAfterUse(GameObject skillInstance, int prefabIndex, float duration)
    {
        yield return new WaitForSeconds(duration);
        skillInstance.SetActive(false);
        GameManager.Instance.skillPool.ReturnToPool(skillInstance, prefabIndex);
    }

    public void UnlockSkill(SkillType skillType)
    {
        if (!unlockedSkills.Contains(skillType))
        {
            unlockedSkills.Add(skillType);
            skillLevels[skillType] = 1; // 기본 레벨
            Debug.Log($"[SkillManager] {skillType} 스킬 해금 완료");
        }
    }

    public List<SkillData> GetUpgradeableSkills()
    {
        List<SkillData> upgradeableSkills = new List<SkillData>();

        foreach (var skillType in unlockedSkills)
        {
            SkillData skillData = skillDatabase.GetSkillData(skillType, currentElement);
            if (skillData != null && skillLevels[skillType] < skillData.maxLevel)
            {
                upgradeableSkills.Add(skillData);
                Debug.Log($"[SkillManager] 업그레이드 가능한 스킬: {skillData.skillName}");
            }
        }

        return upgradeableSkills;
    }

    // 스킬 업그레이드
    // 스킬 업그레이드 메서드
    public void UpgradeSkill(SkillManager.SkillType skillType, ElementType element)
    {
        SkillData skillData = skillDatabase.GetSkillData(skillType, element);
        if (skillData == null)
        {
            Debug.LogError($"[SkillManager] {element} 속성의 {skillType} 스킬 데이터가 없습니다.");
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
        Debug.Log($"[SkillManager] {skillData.skillName} 업그레이드 완료! 현재 레벨: {currentLevel}");
    }

    public List<SkillData> GetLevelUpOptions()
    {
        List<SkillData> options = new List<SkillData>();

        // 이미 해금된 스킬 중 업그레이드 가능한 스킬 추가
        foreach (var skillType in unlockedSkills)
        {
            SkillData skillData = skillDatabase.GetSkillData(skillType, currentElement);
            if (skillData != null && skillLevels[skillType] < skillData.maxLevel)
            {
                options.Add(skillData);
            }
        }

        // 아직 해금되지 않은 스킬 추가
        foreach (SkillData skill in skillDatabase.GetAllSkills())
        {
            if (!unlockedSkills.Contains(skill.skillType))
            {
                options.Add(skill);
            }
        }

        Debug.Log($"[SkillManager] LevelUp Options Count: {options.Count}");
        return options;
    }


    public void UpgradeOrUnlockSkill(SkillData skillData)
    {
        if (unlockedSkills.Contains(skillData.skillType))
        {
            Debug.Log($"[SkillManager] {skillData.skillName} 업그레이드 진행 중...");
            UpgradeSkill(skillData.skillType, skillData.element);
        }
        else
        {
            Debug.Log($"[SkillManager] {skillData.skillName} 해금 진행 중...");
            UnlockSkill(skillData.skillType);
        }
    }
    public HashSet<SkillType> GetUnlockedSkills()
    {
        // 현재 해금된 스킬 목록을 반환
        return new HashSet<SkillType>(unlockedSkills);
    }
    public void release() { }
}