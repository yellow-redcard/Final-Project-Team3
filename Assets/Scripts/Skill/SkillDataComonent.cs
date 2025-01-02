using UnityEngine;

[RequireComponent(typeof(Skill))]
public class SkillDataComponent : MonoBehaviour
{
    public SkillData skillData; // SkillData 스크립터블 오브젝트 연결

    void Awake()
    {
        if (skillData == null)
        {
            Debug.LogError($"[SkillDataComponent] {gameObject.name}에 SkillData가 연결되지 않았습니다!");
        }
    }
public SkillData GetSkillData()
    {
        return skillData;
    }
}
