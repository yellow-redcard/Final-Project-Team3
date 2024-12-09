using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public enum SkillType { Single, Cone, Line, Area }
    public SkillType skillType;

    public float damage = 10f; // 스킬 데미지
    public float range = 5f; // 스킬 범위
    public float duration = 2f; //스킬 지속 시간
    public float speed = 10f; //투사체 속도
    public float cooldown = 3f; //쿨타임

    private bool isReady = true;

    public void UseSkill()
    {
        if (!isReady)
        {
            Debug.LogWarning($"{gameObject.name} 스킬은 아직 쿨타임입니다.");
            return;
        }

        isReady = false;
        StartCoroutine(CooldownRountine());

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

    private IEnumerator CooldownRountine()
    {
        yield return new WaitForSeconds(cooldown);
        isReady = true;
    }

    private void Single()
    {
        Debug.Log($"[Single] 투사체 사용 (Damage: { damage})");
        StartCoroutine(DeactivateAfterDuration());
    }
    private void Cone()
    {
        Debug.Log($"[Cone] 원뿔 모양 투사체 사용 (Damage: {damage})");
        StartCoroutine(DeactivateAfterDuration());
    }
    private void Line()
    {
        Debug.Log($"[Line] 직선 범위 스킬 사용 (Damage: {damage})");
        StartCoroutine(DeactivateAfterDuration());
    }
    private void Area()
    {
        Debug.Log($"[Area] 장판 스킬 사용 (Damage: {damage}, Range: {range})");
        StartCoroutine(DeactivateAfterDuration());
    }
    private IEnumerator DeactivateAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Deactivate();
    }
    public void Deactivate()
    {
        gameObject.SetActive( false );
    }
    public void UpgradeSkill(float damageIncrease, float rangeIncrease, float cooldownDecrease)
    {
        damage += damageIncrease;
        range += rangeIncrease;
        cooldown = Mathf.Max(0.5f, cooldown - cooldownDecrease); // 쿨타임은 최소 0.5초
        Debug.Log($"{skillType} 업그레이드! 데미지: +{damageIncrease}, 범위: +{rangeIncrease}, 쿨타임: -{cooldownDecrease}");
    }

}

