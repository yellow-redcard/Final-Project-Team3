using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed; //속도
    public int health;
    public int maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target; //따라갈 타겟

    bool isLive; //살아있는지

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriter;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() //물리 이동임으로 FixedUpdate에서 진행
    {
        if (!isLive) // 살아있지 않다면 밑에 코드 실행 X
            return;

        Vector2 dirVec = target.position - rigid.position; // 몬스터가 가야할 방향 구하기
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec); // 몬스터를 움직이기
        rigid.velocity = Vector2.zero; // 물리작용에 의한 속도변화를 방지하기 위해 속도를 0으로
    }

    private void LateUpdate()
    {
        if (!isLive) // 살아있지 않다면 밑에 코드 실행 X
            return;

        spriter.flipX = target.position.x < rigid.position.x;
        // 현재위치를 비교해서 x축을 뒤집어준다.
    }

    private void OnEnable()
    {
        target = GameManager.Instance.playerMovement.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }
    public void TakeDamage(int damage) // 체력 감소
    {
        if (!isLive) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die() //사망 처리
    {
        isLive = false;
        gameObject.SetActive(false); // 적 비활성화
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Skill")) // 스킬과 충돌 처리
        {
            Skill skill = collision.GetComponent<Skill>();
            if (skill != null)
            {
                TakeDamage((int)skill.damage);
            }
        }
    }
}
 
