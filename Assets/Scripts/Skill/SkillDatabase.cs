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
        return skillDataList.Find(skill => skill.skillType == type && skill.element == element);
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
    public void TestSkillDatabase()
    {
        foreach (var skill in skillDataList)
        {
            Debug.Log($"Skill Name: {skill.skillName}, Type: {skill.skillType}, Level: {skill.level}/{skill.maxLevel}");
        }
    }
}
