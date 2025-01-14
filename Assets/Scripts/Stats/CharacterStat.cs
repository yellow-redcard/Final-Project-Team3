using UnityEngine;

public enum StatsChangeType
{
    Add,
    Multiple,
    Override
}

[System.Serializable]
public class CharacterStat
{
    public StatsChangeType statsChangeType;
    [Range(1, 3000)] public int maxHealth;
    [Range(1f, 1000f)] public float speed;
    public AttackSO attackSO;
}
