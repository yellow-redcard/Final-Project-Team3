using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Object/SkillData")]
    public class SkillData : ScriptableObject
    {
        public enum ElementType { Water, Fire, Electric, Dark }

        [Header("# Skill Info")]
        public ElementType elementType;
        public string skillName;
        public string skillDescription;
        public Sprite skillIcon;

        [Header("# Skill Stats")]
        public float damageMultiplier;  // 속성별 대미지 배율
        public float skillCooldown;     // 스킬 쿨타임
        public float range;             // 스킬 범위
        public GameObject skillEffect;  // 스킬 효과 프리팹
    }
}
