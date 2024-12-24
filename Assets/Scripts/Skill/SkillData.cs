using System.Collections.Generic;

[System.Serializable]
public class SkillData
{
    public string skillName;
    public SkillManager.SkillType skillType;
    public SkillManager.Element element;
    public float baseDamage;
    public float baseRange;
    public float duration;
    public float cooldown;
    public int maxLevel;
    public int projectileCount; // 단일기 전용

    public Dictionary<string, float> upgradeModifiers; // 업그레이드 옵션

    public SkillData(string name, SkillManager.SkillType type, SkillManager.Element element, float damage, float range, float duration, float cooldown, int maxLevel, int projectiles = 1)
    {
        skillName = name;
        skillType = type;
        this.element = element;
        baseDamage = damage;
        baseRange = range;
        this.duration = duration;
        this.cooldown = cooldown;
        this.maxLevel = maxLevel;
        projectileCount = projectiles;
        upgradeModifiers = new Dictionary<string, float>
        {
            { "Cooldown", -0.5f },
            { "Damage", 5f },
            { "Range", 2f },
            { "Projectile", 1f }
        };
    }
}
