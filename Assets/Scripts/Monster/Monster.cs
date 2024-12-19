using UnityEngine;

public class Monster : MonoBehaviour
{
    public int maxHp = 10; // 몬스터 최대 체력
    private int currentHp;

    private void OnEnable()
    {
        currentHp = maxHp; // 활성화 시 체력 초기화
    }

    public void Initialize()
    {
        currentHp = maxHp;
        // 필요하면 추가적인 초기화 코드 작성
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 스킬과 충돌 처리
        if (collision.CompareTag("Skill"))
        {
            Skill skill = collision.GetComponent<Skill>();
            if (skill != null)
            {
                TakeDamage(skill.baseDamage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= Mathf.FloorToInt(damage);
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 파괴 효과 (필요 시)
        Debug.Log("몬스터 사망");
        GameManager.Instance.monsterPool.ReturnToPool(gameObject, GetMonsterIndex());
    }

    private int GetMonsterIndex()
    {
        for (int i = 0; i < GameManager.Instance.monsterPool.prefabs.Length; i++)
        {
            if (GameManager.Instance.monsterPool.prefabs[i].name == gameObject.name)
            {
                return i;
            }
        }
        return 0; // 기본값 반환
    }
}
