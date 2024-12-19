using UnityEngine;

public class Monster : MonoBehaviour
{
    public int maxHp = 10; // ���� �ִ� ü��
    private int currentHp;

    private void OnEnable()
    {
        currentHp = maxHp; // Ȱ��ȭ �� ü�� �ʱ�ȭ
    }

    public void Initialize()
    {
        currentHp = maxHp;
        // �ʿ��ϸ� �߰����� �ʱ�ȭ �ڵ� �ۼ�
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��ų�� �浹 ó��
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
        // �ı� ȿ�� (�ʿ� ��)
        Debug.Log("���� ���");
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
        return 0; // �⺻�� ��ȯ
    }
}
