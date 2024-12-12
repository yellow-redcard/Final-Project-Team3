using UnityEngine;

public enum MonsterType
{
    Boss,
    Normal
}
public enum AttackType
{
    ShortRange,
    LongRange
}

[CreateAssetMenu(fileName = "MonsterStat", menuName = "ScriptableObject/Monster")]
public class MonsterStatSO : ScriptableObject
{
    public MonsterType monsterType;
    public AttackType attackType;

    public float hp;//ü��
    public float attackBody;//���ݷ�
    public float attackBullet;//���ݷ�
    public float attackDelay;//���ݵ�����
    public float speed;//�̵��ӵ�
    public float knockBackPower;//�˹��Ŀ�
    public float knockBackResistance;//�˹����׷�
    public float distance;//���ݻ�Ÿ�
}
