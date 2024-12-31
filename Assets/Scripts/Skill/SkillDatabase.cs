using System.Collections.Generic;
using UnityEngine;

public class SkillDatabase : MonoBehaviour
{
    [SerializeField]
    private List<SkillData> skillDataList; // ScriptableObject 리스트

    /// <summary>
    /// 특정 SkillType과 Element에 해당하는 SkillData를 반환합니다.
    /// </summary>
    public SkillData GetSkillData(SkillManager.SkillType type, ElementType element)
    {
        SkillData skillData = skillDataList.Find(skill => skill.skillType == type && skill.element == element);

        if (skillData == null)
        {
            Debug.LogError($"[SkillDatabase] {element} 속성의 {type} 스킬 데이터가 없습니다!");
        }
        else
        {
            Debug.Log($"[SkillDatabase] {skillData.skillName} 데이터 로드 성공");
        }

        return skillData;
    }

    /// <summary>
    /// 모든 스킬 데이터를 반환합니다.
    /// </summary>
    public List<SkillData> GetAllSkills()
    {
        return skillDataList;
    }

    /// <summary>
    /// 디버그용: 모든 스킬 정보를 출력합니다.
    /// </summary>
    public void ValidateSkillDatabase()
    {
        foreach (var skill in skillDataList)
        {
            if (skill.levelUpStats.Length != skill.maxLevel)
            {
                Debug.Log($"[SkillDatabase] Loaded Skill: {skill.skillName}, Element: {skill.element}");
            }
            else
            {
                Debug.Log($"[SkillDatabase] 스킬 '{skill.skillName}' 검증 완료.");
            }
        }
    }
}
