using System.Collections;
using UnityEngine;
using static SkillManager;

public class Skill : MonoBehaviour
{
    public SkillType skillType;

    public float baseDamage = 10f;    // 기본 데미지
    public float baseRange = 5f;      // 기본 범위
    public float duration = 2f;       // 스킬 지속 시간
    public float cooldown = 3f;       // 스킬 쿨타임
    public int projectileCount = 1;   // 투사체 개수 (단일기 전용)

    private int level = 1;            // 스킬 레벨
    private bool isReady = true;

    public void UseSkill()
    {
        if (!isReady) return;

        isReady = false;
        StartCoroutine(CooldownRoutine());

        float damage = baseDamage * level;
        float range = baseRange * level;

        switch (skillType)
        {
            case SkillType.Single:
                Debug.Log($"[Single] 데미지: {damage}, 투사체: {projectileCount}");
                break;
            case SkillType.Cone:
                Debug.Log($"[Cone] 원뿔 데미지: {damage}, 범위: {range}");
                break;
            case SkillType.Line:
                Debug.Log($"[Line] 일직선 데미지: {damage}, 범위: {range}");
                break;
            case SkillType.Area:
                Debug.Log($"[Area] 장판 데미지: {damage}, 범위: {range}");
                break;
        }

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
    public void UpgradeSkill(string option)
    {
        switch (option)
        {
            case "Cooldown":
                cooldown = Mathf.Max(0.5f, cooldown - 0.5f);
                Debug.Log($"{skillType} 쿨타임 감소: {cooldown}");
                break;
            case "Damage":
                baseDamage += 5f;
                Debug.Log($"{skillType} 데미지 증가: {baseDamage}");
                break;
            case "Range":
                baseRange += 2f;
                Debug.Log($"{skillType} 범위 증가: {baseRange}");
                break;
            case "Projectile":
                if (skillType == SkillType.Single)
                {
                    projectileCount++;
                    Debug.Log($"{skillType} 투사체 개수 증가: {projectileCount}");
                }
                break;
        }
    }
}
