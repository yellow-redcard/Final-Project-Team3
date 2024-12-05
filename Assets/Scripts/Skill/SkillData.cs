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
        public float damageMultiplier;  // �Ӽ��� ����� ����
        public float skillCooldown;     // ��ų ��Ÿ��
        public float range;             // ��ų ����
        public GameObject skillEffect;  // ��ų ȿ�� ������
    }
}
