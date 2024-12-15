using System.Collections;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public enum SkillType { Single, Cone, Line, Area }
    public SkillType skillType;

    public float baseDamage = 10f;
    public float baseRange = 5f;
    public float duration = 2f;
    public float speed = 10f;
    public float cooldown = 3f;

    private int level = 1;
    private bool isReady = true;

    public void SetLevel(int newLevel)
    {
        level = newLevel;
    }

    public void UseSkill()
    {
        if (!isReady)
        {
            Debug.LogWarning($"{gameObject.name} 스킬은 쿨타임입니다.");
            return;
        }

        isReady = false;
        StartCoroutine(CooldownRoutine());

        switch (skillType)
        {
            case SkillType.Single:
                Single();
                break;
            case SkillType.Cone:
                Cone();
                break;
            case SkillType.Line:
                Line();
                break;
            case SkillType.Area:
                Area();
                break;
        }
    }

    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldown);
        isReady = true;
    }

    private void Single()
    {
        float damage = baseDamage * level;
        Debug.Log($"[Single] 타겟 공격 (Damage: {damage})");
        StartCoroutine(DeactivateAfterDuration());
    }

    private void Cone()
    {
        float damage = baseDamage * level;
        Debug.Log($"[Cone] 원뿔 스킬 발사 (Damage: {damage})");
        StartCoroutine(DeactivateAfterDuration());
    }

    private void Line()
    {
        float damage = baseDamage * level;
        Debug.Log($"[Line] 일직선 스킬 발사 (Damage: {damage})");
        StartCoroutine(DeactivateAfterDuration());
    }

    private void Area()
    {
        float damage = baseDamage * level;
        float range = baseRange * level;
        Debug.Log($"[Area] 장판 스킬 발사 (Damage: {damage}, Range: {range})");
        StartCoroutine(DeactivateAfterDuration());
    }

    private IEnumerator DeactivateAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Deactivate();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
