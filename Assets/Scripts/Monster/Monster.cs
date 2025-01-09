using System;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int maxHp = 10; // 몬스터 최대 체력
    private int currentHp;
    public static event EventHandler OnMonsterDie;
    [SerializeField] private ElementType monsterElementType;
    [SerializeField] private GameObject[] dropItems; // 드롭 아이템 배열 추가
    [SerializeField] private GameObject dropGold;
    private ElementSystem elementSystem;

    private void Start()
    {
        monsterElementType = (ElementType)UnityEngine.Random.Range(1, System.Enum.GetValues(typeof(ElementType)).Length);
        elementSystem = gameObject.AddComponent<ElementSystem>();
    }

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
                float damage = skill.baseDamage;
                elementSystem.DetermineOutcome(skill.currentElement, monsterElementType, ref damage); // 스킬과 몬스터의 속성 타입 비교
                TakeDamage(damage);
            }
        }
        //데미지 처리
        if (collision.TryGetComponent(out Slime slime))
        {
            Debug.Log("슬라임 데미지");
            slime.Damage();
            //if ()
            //{
                    //slime.BossDamage();
            //}
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

    public void Die()
    {
        OnMonsterDie?.Invoke(this, EventArgs.Empty);
        GameManager.Instance.monsterKill += 1;
        // 파괴 효과 (필요 시)
        Debug.Log("몬스터 사망");
        DropGold();
        DropItem();
        GameManager.Instance.monsterPool.ReturnToPool(gameObject, GetMonsterIndex());

    }

    private void DropItem()
    {
        if (CompareTag("Mimic"))
        {
            if (dropItems.Length > 0)
            {
                // 랜덤 아이템 선택
                GameObject drop = dropItems[UnityEngine.Random.Range(0, dropItems.Length)];

                // 드롭 아이템 인스턴스 생성
                Instantiate(drop, transform.position, Quaternion.identity); // 현재 위치에 드롭
            }
        }
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

    private void DropGold()
    {
        Instantiate(dropGold, transform.position, Quaternion.identity);
    }
}