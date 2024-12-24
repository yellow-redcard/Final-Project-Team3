using System.Collections;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public SkillManager.SkillType skillType;
    public float baseDamage;
    public float baseRange;
    public float duration;
    public float cooldown;
    public int projectileCount;

    private int level = 1;
    private bool isReady = true;

    public void UseSkill()
    {
        if (!isReady) return;
        isReady = false;
        StartCoroutine(CooldownRoutine());

        float damage = baseDamage * level;
        float range = baseRange * level;

        Debug.Log($"[{skillType}] 데미지: {damage}, 범위: {range}");
        StartCoroutine(DeactivateAfterDuration());
    }

    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldown);
        isReady = true;
    }

    private IEnumerator DeactivateAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
