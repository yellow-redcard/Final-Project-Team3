using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillData", menuName = "Skill/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;                // 스킬 이름
    public SkillManager.SkillType skillType; // 스킬 타입 (Single, Cone 등)
    public SkillManager.Element element;    // 스킬 속성 (Fire, Water 등)

    public float baseDamage;                // 기본 데미지
    public float baseRange;                 // 기본 사거리
    public float cooldown;                  // 기본 쿨다운
    public float duration;                  // 지속 시간
    public int level;
    public int maxLevel;                    // 최대 레벨
    public int projectileCount;             // 투사체 개수
    public string upgradeDescription;       // 업그레이드 설명

    // 레벨별 데이터
    [System.Serializable]
    public class LevelUpStats
    {
        public float damage;       // 레벨별 데미지
        public float cooldown;     // 레벨별 쿨다운
        public float range;        // 레벨별 사거리
        public int projectileCount; // 레벨별 투사체 수
    }

    public LevelUpStats[] levelUpStats; // 레벨별 데이터 배열
}
