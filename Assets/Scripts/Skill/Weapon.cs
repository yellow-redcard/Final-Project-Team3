using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id; // PoolManager���� ����� ��ų ID
    public float damage;
    public float fireRate;
    public float projectileSpeed;

    private float fireTimer;
    private Scanner scanner;

    private void Awake()
    {
        scanner = GetComponent<Scanner>();
    }

    private void Update()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            Fire();
        }
    }

    private void Fire()
    {
        if (scanner.nearestTarget == null) return;

        Vector3 direction = (scanner.nearestTarget.position - transform.position).normalized;

        // PoolManager���� ��ų ����
        GameObject projectile = GameManager.Instance.poolManager.Get(id);
        projectile.transform.position = transform.position;
        projectile.SetActive(true);

        // ��ų �ʱ�ȭ
        Skill skill = projectile.GetComponent<Skill>();
        if (skill != null)
        {
            skill.Init(damage, direction, projectileSpeed);
        }
    }
}

