using System;
using Unity.VisualScripting;
using UnityEngine;

public enum SkillType { Single, Cone, Line, Zone } // 스킬 종류
public enum Attribute {  Water, Fire, Electric, Dark } //스킬 속성
public class SkillData
{
    public SkillType skillType;
    public Attribute attribute;
    public float damage;
    public float range;
    public float cooldown;
    public int projectileCount;

    public SkillData(SkillType skillType, Attribute attribute, float damage, float cooldown, float range, int projectileCount)
    {
        this.skillType = skillType;
        this.attribute = attribute;
        this.damage = damage;
        this.cooldown = cooldown;
        this.range = range;
        this.projectileCount = projectileCount;
    }

    public void UpgradeDamage(float amount) => damage += amount;
    public void UpgradeCooldown(float amount) => cooldown = Mathf.Max(0.1f, cooldown - amount);
    public void UpgradeRange(float amount) => range += amount;
    public void UpgradeProjectileCount(int amount) => projectileCount += amount;
}

