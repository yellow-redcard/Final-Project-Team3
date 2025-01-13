using System.Collections;
using UnityEditor.Experimental.GraphView;
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
    public SkillData skillData;

    private Transform player; // 플레이어 참조
    private bool isReady = true;
    private bool isActive = false;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Awake()
    {
        if (GameManager.Instance != null && GameManager.Instance.skillManager != null)
        {
            currentElement = GameManager.Instance.skillManager.currentElement;
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

    private void ActivateParticleSystem()
    {
        var particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }

    private void DeactivateParticleSystem()
    {
        var particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Stop();
        }
    }

    private IEnumerator TargetedSkillRoutine()
    {
        DealDamageToEnemies();
        ActivateParticleSystem();

        yield return new WaitForSeconds(duration);

        DeactivateParticleSystem();
        gameObject.SetActive(false); // 스킬 종료 후 비활성화
    }


    private IEnumerator AreaSkillRoutine()
    {
        ActivateParticleSystem();

        float elapsedTime = 0f;
        float damageInterval = 1f; // 1초마다 데미지 주기

        while (elapsedTime < duration)
        {
            // 1초마다 데미지 주기
            if (elapsedTime % damageInterval < Time.deltaTime)
            {
                DealDamageToEnemies();
            }

            FollowPlayer(); // 플레이어를 따라감
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        DeactivateParticleSystem();
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
                    float damage = baseDamage;
                    monster.elementSystem.DetermineOutcome(currentElement, monster.monsterElementType, ref damage); // 스킬과 몬스터의 속성 타입 비교
                    monster.TakeDamage(damage);
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
    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
    }
}
