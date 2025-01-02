using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Skill : MonoBehaviour
{
    public SkillManager.SkillType skillType;
    public ElementType currentElement;
    public float baseDamage;
    public float baseRange;
    public float duration;
    public float cooldown;
    public int projectileCount;

    private Transform player; // 플레이어 참조
    private bool isReady = true;
    private bool isActive = false;

    private void Awake()
    {
        if (GameManager.Instance != null && GameManager.Instance.skillManager != null)
        {
            currentElement = GameManager.Instance.skillManager.currentElement;
        }
        else
        {
            Debug.LogError("[Skill] GameManager 또는 SkillManager 초기화 실패.");
        }
    }
    public void Configure(SkillData skillData, Transform playerTransform)
    {
        baseDamage = skillData.baseDamage;
        baseRange = skillData.baseRange;
        cooldown = skillData.cooldown;
        duration = skillData.duration;
        projectileCount = skillData.projectileCount;

        player = playerTransform; // 플레이어 위치 설정
        currentElement = skillData.element;
    }
    public void UseSkill()
    {
        if (skillType == SkillManager.SkillType.Area)
        {
            StartCoroutine(AreaSkillRoutine());
        }
        else
        {
            StartCoroutine(TargetedSkillRoutine());
        }
    }

    private IEnumerator TargetedSkillRoutine()
    {
        DealDamageToEnemies();

        var particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
        }

        yield return new WaitForSeconds(duration); // duration 동안 대기

        if (particleSystem != null)
        {
            particleSystem.Stop();
        }

        gameObject.SetActive(false); // 스킬 종료 후 비활성화
    }


    private IEnumerator AreaSkillRoutine()
    {
        var particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
        }

        DealDamageToEnemies();
        yield return new WaitForSeconds(duration); // duration 동안 대기

        if (particleSystem != null)
        {
            particleSystem.Stop();
        }

        gameObject.SetActive(false); // 스킬 종료 후 비활성화
    }

    private void DealDamageToEnemies()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, baseRange);
        foreach (var enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                var monster = enemy.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.TakeDamage(baseDamage);
                }
            }
        }
    }
    private void FollowPlayer()
    {
        if (player != null)
        {
            transform.position = player.position;
        }
    }
    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldown);
        isReady = true;
    }

    private IEnumerator DeactivateAfterDuration(GameObject skillInstance, float duration)
    {
        yield return new WaitForSeconds(duration);

        var particleSystem = skillInstance.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Stop();
        }
        skillInstance.SetActive(false); // 오브젝트 비활성화
    }
    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
    }
}
