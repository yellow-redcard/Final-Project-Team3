using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public enum SkillType { Single, Cone, Line, Area }
    public SkillType skillType;

    public float damage = 10f; // ��ų ������
    public float range = 5f; // ��ų ����
    public float duration = 2f; //��ų ���� �ð�
    public float speed = 10f; //����ü �ӵ�
    public float cooldown = 3f; //��Ÿ��

    private bool isReady = true;

    public void UseSkill()
    {
        if (!isReady)
        {
            Debug.LogWarning($"{gameObject.name} ��ų�� ���� ��Ÿ���Դϴ�.");
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
        Debug.Log($"[Single] ����ü ��� (Damage: { damage})");
        StartCoroutine(DeactivateAfterDuration());
    }
    private void Cone()
    {
        Debug.Log($"[Cone] ���� ��� ����ü ��� (Damage: {damage})");
        StartCoroutine(DeactivateAfterDuration());
    }
    private void Line()
    {
        Debug.Log($"[Line] ���� ���� ��ų ��� (Damage: {damage})");
        StartCoroutine(DeactivateAfterDuration());
    }
    private void Area()
    {
        Debug.Log($"[Area] ���� ��ų ��� (Damage: {damage}, Range: {range})");
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
        cooldown = Mathf.Max(0.5f, cooldown - cooldownDecrease); // ��Ÿ���� �ּ� 0.5��
        Debug.Log($"{skillType} ���׷��̵�! ������: +{damageIncrease}, ����: +{rangeIncrease}, ��Ÿ��: -{cooldownDecrease}");
    }

}

