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
    private SoundManager soundManager;
    private Dictionary<ElementType, Dictionary<SkillType, int>> skillPrefabIndices;
    private HashSet<SkillType> unlockedSkills = new HashSet<SkillType> { SkillType.Single }; // 기본 스킬 포함
    private Dictionary<SkillType, float> skillCooldownTimers = new Dictionary<SkillType, float>(); // 쿨다운 타이머
    private Dictionary<SkillType, int> skillLevels = new Dictionary<SkillType, int>();
    public ElementType currentElement = ElementType.None;
    public List<GameObject> skillPrefabs;
    private List<Skill> skills;  // 'skills' 리스트 선언

    public List<SkillData> allSkills;  // 모든 스킬 데이터 목록 (SkillData 배열을 연결)
    public SkillDatabase skillDatabase;
    private bool isFiring = false;

    private const string SKILL_LEVEL_PREFIX = "SkillLevel_";
    public void init()
    {
        skillPrefabIndices = new Dictionary<ElementType, Dictionary<SkillType, int>>();

        ResetSkillLevels();

        foreach (SkillType skillType in System.Enum.GetValues(typeof(SkillType)))
        {
            skillLevels[skillType] = 1; // 모든 스킬 초기 레벨 설정
            skillCooldownTimers[skillType] = 0f; // 초기 쿨다운
        }

        skillDatabase = FindObjectOfType<SkillDatabase>();
        if (skillDatabase == null)
        {
            return;
        }
        soundManager = FindObjectOfType<SoundManager>();
        
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
            yield break;
        }
       
        while (true)
        {
            List<Transform> enemies = monsterPoolManager.GetActiveMonsters();
            enemies.RemoveAll(enemy => enemy == null || !enemy.gameObject.activeSelf); // 삭제된 적 제거

            foreach (SkillType skillType in unlockedSkills)
            {
                if (skillCooldownTimers[skillType] <= 0)
                {
                    FireSkill(skillType, GameManager.Instance.player.position, enemies);
                    ResetSkillCooldown(skillType);
                }
            }

            UpdateCooldownTimers(); // 쿨다운 업데이트
            yield return null;
        }
    }
    private void UpdateCooldownTimers()
    {
        foreach (var key in skillCooldownTimers.Keys.ToArray()) // 배열로 복사하여 순회
        {
            if (skillCooldownTimers.TryGetValue(key, out float cooldown) && cooldown > 0)
            {
                skillCooldownTimers[key] = cooldown - Time.deltaTime;
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
        SkillData skillData = skillDatabase.GetSkillData(skillType, GameManager.Instance.skillManager.currentElement);
        if (skillData != null && enemies.Count > 0 && skillCooldownTimers[skillType] <= 0)
        {
            Vector3 spawnPosition = skillType == SkillType.Area ? playerPosition : GetTargetPosition(enemies, playerPosition);
            soundManager.PlaySkillSound(skillData);
            SpawnSkill(skillType, spawnPosition, skillData);
            ResetSkillCooldown(skillType);
        }
    }

        private Vector3 GetTargetPosition(List<Transform> enemies, Vector3 playerPosition)
    {
        if (enemies == null || enemies.Count == 0) return playerPosition;

        Transform closestEnemy = enemies[0];
        float closestDistance = Vector3.Distance(playerPosition, closestEnemy.position);

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(playerPosition, enemy.position);
            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }

        return closestEnemy.position;
    }

    private void SpawnSkill(SkillType skillType, Vector3 position, SkillData skillData)
    {
        if (skillPrefabIndices.ContainsKey(currentElement) && skillPrefabIndices[currentElement].ContainsKey(skillType))
        {
            int prefabIndex = skillPrefabIndices[currentElement][skillType];
            GameObject skillInstance = GameManager.Instance.skillPool.Get(prefabIndex);

            if (skillInstance != null)
            {
                // 직선 스킬만 플레이어 기준으로 위치 설정
                if (skillType == SkillType.Line)
                {
                    skillInstance.transform.position = GameManager.Instance.player.position;
                }
                else
                {
                    skillInstance.transform.position = position;
                }

                skillInstance.SetActive(true);

                Skill skill = skillInstance.GetComponent<Skill>();
                if (skill != null)
                {
                    skill.Initialize(GameManager.Instance.player); // 플레이어 기준
                    skill.baseDamage = skillData.baseDamage;
                    skill.cooldown = skillData.cooldown;
                    skill.baseRange = skillData.baseRange;
                    skill.duration = skillData.duration;
                    skill.projectileCount = skillData.projectileCount;

                    AudioSource skillAudioSource = skillInstance.GetComponent<AudioSource>();
                    if (skillAudioSource == null)
                    {
                        skillAudioSource = skillInstance.AddComponent<AudioSource>();
                    }
                    skillAudioSource.clip = skillData.skillSound;
                    skillAudioSource.loop = false; // 반복 재생 안 함
                    skillAudioSource.Play();

                    skill.UseSkill();
                    StartCoroutine(ReturnToPool(skillInstance, prefabIndex, skillData.duration, skillAudioSource));
                }
            }
        }
    }

    private IEnumerator ReturnToPool(GameObject skillInstance, int prefabIndex, float duration, AudioSource audioSource)
    {
        yield return new WaitForSeconds(duration);

        // 사운드 정지
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // 스킬 오브젝트 비활성화 및 반환
        skillInstance.SetActive(false);
        GameManager.Instance.skillPool.ReturnToPool(skillInstance, prefabIndex);
    }

    public void UnlockSkill(SkillType skillType)
    {
        if (!unlockedSkills.Contains(skillType))
        {
            unlockedSkills.Add(skillType);
            skillLevels[skillType] = 1; // 기본 레벨
            PlayerPrefs.SetInt(SKILL_LEVEL_PREFIX + skillType.ToString(), 1); // PlayerPrefs에 저장
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
            }
        }

        return upgradeableSkills;
    }

    // 스킬 업그레이드
    // 스킬 업그레이드 메서드
    public void UpgradeSkill(SkillType skillType, ElementType element)
    {
        SkillData skillData = skillDatabase.GetSkillData(skillType, element);
        if (skillData == null)
        {
            return;
        }

        int currentLevel = skillLevels[skillType];
        if (currentLevel >= skillData.maxLevel)
        {
            return;
        }

        // 레벨 업
        skillLevels[skillType]++;
        PlayerPrefs.SetInt(SKILL_LEVEL_PREFIX + skillType.ToString(), skillLevels[skillType]); // PlayerPrefs에 레벨 저장
    }
    public List<SkillData> GetLevelUpOptions()
    {
        List<SkillData> options = new List<SkillData>();

        // 현재 슬라임 속성을 기준으로 스킬 데이터 필터링
        ElementType currentElement = GameManager.Instance.skillManager.currentElement;

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
            if (!unlockedSkills.Contains(skill.skillType) && skill.element == currentElement)
            {
                options.Add(skill);
            }
        }

        return options;
    }


    public void UpgradeOrUnlockSkill(SkillData selectedSkill)
    {
        if (unlockedSkills.Contains(selectedSkill.skillType))
        {
            // 스킬 업그레이드
            UpgradeSkill(selectedSkill.skillType, selectedSkill.element);
            selectedSkill.level++; // 선택된 스킬 레벨 업데이트

            // 다른 스킬들의 레벨도 함께 업데이트
            foreach (var skill in skillDatabase.GetAllSkills())
            {
                if (skill.skillType == selectedSkill.skillType && skill.element == selectedSkill.element)
                {
                    skill.level = selectedSkill.level;
                }
            }
        }
        else
        {
            // 스킬 해금
            UnlockSkill(selectedSkill.skillType);
            selectedSkill.level = 1; // 새로 해금된 스킬의 레벨을 1로 설정

            // 다른 스킬들의 레벨도 함께 업데이트
            foreach (var skill in skillDatabase.GetAllSkills())
            {
                if (skill.skillType == selectedSkill.skillType && skill.element == selectedSkill.element)
                {
                    skill.level = 1;
                }
            }
        }
    }
    public HashSet<SkillType> GetUnlockedSkills()
    {
        // 현재 해금된 스킬 목록을 반환
        return new HashSet<SkillType>(unlockedSkills);
    }
    public void ResetSkills()
    {
        foreach (var skill in skills)
        {
            if (skill != null && skill.skillData != null)
            {
                skill.skillData.level = 0; // 레벨 초기화
                skill.skillData.upgradeDescription = ""; // 업그레이드 설명 초기화 (선택 사항)
            }
        }
    }
    private void ResetSkillLevels()
    {
        // 게임 시작 시 스킬 레벨을 1로 초기화 (SkillData의 level 값 초기화)
        foreach (var skill in allSkills)
        {
            skill.level = 1; // 모든 스킬의 level을 1로 설정
        }

        // PlayerPrefs에 스킬 레벨 초기화
        foreach (var skillType in skillLevels.Keys)
        {
            PlayerPrefs.SetInt(SKILL_LEVEL_PREFIX + skillType.ToString(), 1); // PlayerPrefs에서 레벨 초기화
        }
    }

    public void release() { }
}