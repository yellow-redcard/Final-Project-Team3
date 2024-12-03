using UnityEngine;

public class Skill : MonoBehaviour
{
    public float damage;
    public int projectileCount;
    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, Vector3 direction, float speed)
    {
        this.damage = damage;
        rigid.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // ���� �浹 
        {
            // �������� Enemy ��ũ��Ʈ���� ó��
            DisableSkill();
        }
        else if (collision.CompareTag("Wall")) // �� �� ��ֹ��� �浹 
        {
            DisableSkill();
        }
    }

    private void DisableSkill()
    {
        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false); // ��Ȱ��ȭ �� Pool�� ��ȯ
    }
}
