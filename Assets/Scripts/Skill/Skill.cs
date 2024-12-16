using System.Collections;
using UnityEngine;
using static SkillManager;

public class Skill : MonoBehaviour
{
    public SkillType skillType;

    public float baseDamage = 10f;    // �⺻ ������
    public float baseRange = 5f;      // �⺻ ����
    public float duration = 2f;       // ��ų ���� �ð�
    public float cooldown = 3f;       // ��ų ��Ÿ��
    public int projectileCount = 1;   // ����ü ���� (���ϱ� ����)

    private int level = 1;            // ��ų ����
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
                Debug.Log($"[Single] ������: {damage}, ����ü: {projectileCount}");
                break;
            case SkillType.Cone:
                Debug.Log($"[Cone] ���� ������: {damage}, ����: {range}");
                break;
            case SkillType.Line:
                Debug.Log($"[Line] ������ ������: {damage}, ����: {range}");
                break;
            case SkillType.Area:
                Debug.Log($"[Area] ���� ������: {damage}, ����: {range}");
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
                Debug.Log($"{skillType} ��Ÿ�� ����: {cooldown}");
                break;
            case "Damage":
                baseDamage += 5f;
                Debug.Log($"{skillType} ������ ����: {baseDamage}");
                break;
            case "Range":
                baseRange += 2f;
                Debug.Log($"{skillType} ���� ����: {baseRange}");
                break;
            case "Projectile":
                if (skillType == SkillType.Single)
                {
                    projectileCount++;
                    Debug.Log($"{skillType} ����ü ���� ����: {projectileCount}");
                }
                break;
        }
    }
}
