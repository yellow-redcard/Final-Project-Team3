using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed; //�ӵ�
    public int health;
    public int maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target; //���� Ÿ��

    bool isLive; //����ִ���

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriter;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() //���� �̵������� FixedUpdate���� ����
    {
        if (!isLive) // ������� �ʴٸ� �ؿ� �ڵ� ���� X
            return;

        Vector2 dirVec = target.position - rigid.position; // ���Ͱ� ������ ���� ���ϱ�
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec); // ���͸� �����̱�
        rigid.velocity = Vector2.zero; // �����ۿ뿡 ���� �ӵ���ȭ�� �����ϱ� ���� �ӵ��� 0����
    }

    private void LateUpdate()
    {
        if (!isLive) // ������� �ʴٸ� �ؿ� �ڵ� ���� X
            return;

        spriter.flipX = target.position.x < rigid.position.x;
        // ������ġ�� ���ؼ� x���� �������ش�.
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
    public void TakeDamage(int damage) // ü�� ����
    {
        if (!isLive) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die() //��� ó��
    {
        isLive = false;
        gameObject.SetActive(false); // �� ��Ȱ��ȭ
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Skill")) // ��ų�� �浹 ó��
        {
            Skill skill = collision.GetComponent<Skill>();
            if (skill != null)
            {
                TakeDamage((int)skill.damage);
            }
        }
    }
}
 
