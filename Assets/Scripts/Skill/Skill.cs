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
        if (collision.CompareTag("Enemy")) // 적과 충돌 
        {
            // 데미지는 Enemy 스크립트에서 처리
            DisableSkill();
        }
        else if (collision.CompareTag("Wall")) // 벽 등 장애물과 충돌 
        {
            DisableSkill();
        }
    }

    private void DisableSkill()
    {
        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false); // 비활성화 후 Pool로 반환
    }
}
