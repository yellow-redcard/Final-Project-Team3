using System.Collections;
using UnityEngine;

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
        // GameManager.Instance 초기화가 보장된 시점에 currentElement 설정
        if (GameManager.Instance != null && GameManager.Instance.skillManager != null)
        {
            currentElement = GameManager.Instance.skillManager.currentElement;
        }
        else
        {
            Debug.LogError("[Skill] GameManager 또는 SkillManager가 초기화되지 않았습니다.");
        }
    }

    private void Update()
    {
        if (skillType == SkillManager.SkillType.Area && isActive)
        {
            FollowPlayer(); // Area 스킬은 플레이어를 따라다님
        }
    }

    public void UseSkill()
    {
        if (!isReady) return;

        isReady = false;
        isActive = true;

        if (skillType == SkillManager.SkillType.Area)
        {
            StartCoroutine(AreaSkillRoutine());
        }
        else
        {
            StartCoroutine(CooldownRoutine());
            StartCoroutine(DeactivateAfterDuration());
        }
    }

    private void FollowPlayer()
    {
        if (player != null)
        {
            transform.position = player.position;
        }
    }

    private IEnumerator AreaSkillRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            DealDamageToEnemies(); // 데미지 계산
            elapsedTime += 1f; // 데미지 주기 (1초)
            yield return new WaitForSeconds(1f);
        }

        isActive = false;
        StartCoroutine(CooldownRoutine());
        gameObject.SetActive(false); // 스킬 비활성화
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

    public void Initialize(Transform playerTransform)
    {
        player = playerTransform; // 플레이어 참조 설정
    }
}
