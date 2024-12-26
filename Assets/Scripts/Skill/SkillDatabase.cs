using System.Collections.Generic;
using UnityEngine;

public class SkillDatabase : MonoBehaviour
{
    [SerializeField] // 인스펙터에서 접근 가능하도록 설정
    private List<SkillData> skillDataList = new List<SkillData>();

    /// <summary>
    /// 특정 SkillType과 Element에 해당하는 SkillData를 반환합니다.
    /// </summary>
    /// <param name="type">스킬 타입</param>
    /// <param name="element">스킬 속성</param>
    /// <returns>SkillData 또는 null</returns>
    public SkillData GetSkillData(SkillManager.SkillType type, SkillManager.Element element)
    {
        return skillDataList.Find(skill => skill.skillType == type && skill.element == element);
    }

    /// <summary>
    /// 모든 스킬 데이터를 반환합니다.
    /// </summary>
    /// <returns>SkillData 리스트</returns>
    public List<SkillData> GetAllSkills()
    {
        return skillDataList;
    }
    public void TestSkillDatabase()
    {
        foreach (var skill in skillDataList)
        {
            Debug.Log($"[SkillDatabase] 스킬 이름: {skill.skillName}, 타입: {skill.skillType}, 속성: {skill.element}");
        }
    }
}
