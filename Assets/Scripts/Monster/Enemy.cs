using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed; //�ӵ�
    public Rigidbody2D target; //���� Ÿ��

    bool isLive = true; //����ִ���

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
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
    }
}   
