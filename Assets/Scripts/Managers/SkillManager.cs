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
    private List<Skill> activeSkills = new List<Skill>(); // Ȱ��ȭ�� ��ų ���
    private Element currentElement;

    public void init()
    {
        skillPrefabIndices = new Dictionary<Element, Dictionary<SkillType, int>>();
        foreach (SkillType skillType in System.Enum.GetValues(typeof(SkillType)))
        {
            skillLevels[skillType] = 1; // ��� ��ų ���� �ʱ�ȭ
        }
        LoadSkillPrefabs();
        UnlockSkill(SkillType.Single); // �⺻ ��ų �ر�
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

        if (skillType == SkillType.Area) // ���� ��ų�� �÷��̾� ��ġ�� ����
        {
            SpawnSkill(skillType, playerPosition);
        }
        else // ���ϱ�, ����, �������� �� ������ ���͸� Ÿ����
        {
            Transform targetEnemy = GetSingleTarget(enemies); // �� ������ ���� ����
            if (targetEnemy != null)
            {
                SpawnSkill(skillType, targetEnemy.position);
            }
        }
    }
    private Transform GetSingleTarget(List<Transform> enemies)
    {
        if (enemies == null || enemies.Count == 0) return null;

        // ���� ����� ���͸� �������� Ÿ����
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

        return closestEnemy; // ���� ����� ���� ��ȯ
    }
    private void SpawnSkill(SkillType skillType, Vector3 position)
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
            Debug.Log($"��ų {skillType} �ر�!");
        }
    }

    // ��ų ���׷��̵�
    public void UpgradeSkill(SkillType skillType, string option)
    {
        if (!unlockedSkills.Contains(skillType)) return;

        skillLevels[skillType]++;
        Debug.Log($"��ų {skillType} ���� ��! ���� ����: {skillLevels[skillType]}");
    }

    public int GetSkillLevel(SkillType skillType)
    {
        return skillLevels.ContainsKey(skillType) ? skillLevels[skillType] : 0;
    }

    public HashSet<SkillType> GetUnlockedSkills()
    {
        return new HashSet<SkillType>(unlockedSkills);
    }

    // �߻� �ӵ� ���� �޼��� ����
    public void AdjustFireRate(float rate)
    {
        foreach (var skill in activeSkills)
        {
            skill.cooldown = Mathf.Max(0.1f, skill.cooldown * (1f - rate)); // �ּ� ��Ÿ�� 0.1��
        }

        Debug.Log($"[SkillManager] ��� ��ų�� �߻� �ӵ��� {rate * 100}% ��ŭ �����߽��ϴ�.");
    }

    public void release()
    {
        activeSkills.Clear();
    }
    public List<(SkillType, string)> GetUpgradeOptions()
    {
        List<(SkillType, string)> options = new List<(SkillType, string)>();

        // ��� ������ ��ų�� �� ���׷��̵� �ɼ� �߰�
        foreach (var skill in unlockedSkills)
        {
            switch (skill)
            {
                case SkillType.Single:
                    options.Add((skill, "Cooldown"));
                    options.Add((skill, "Damage"));
                    options.Add((skill, "Projectile"));
                    break;

                case SkillType.Cone:
                case SkillType.Line:
                    options.Add((skill, "Cooldown"));
                    options.Add((skill, "Damage"));
                    options.Add((skill, "Range"));
                    break;

                case SkillType.Area:
                    options.Add((skill, "Damage"));
                    options.Add((skill, "Range"));
                    break;
            }

            // �ɼ� 3���� ����
            if (options.Count >= 3) break;
        }

        return options;
    }
}
