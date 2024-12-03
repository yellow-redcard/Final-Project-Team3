using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public float damage;
    public int projectileCount;
    public float fireRate;

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

    public void Init(int id, float damage, int projectileCount, float fireRate)
    {
        this.id = id;
        this.damage = damage;
        this.projectileCount = projectileCount;
        this.fireRate = fireRate;
    }

    private void Fire()
    {
        if (scanner.nearestTarget == null) return;

        Vector3 direction = (scanner.nearestTarget.position - transform.position).normalized;
        for (int i = 0; i < projectileCount; i++)
        {
            CreateProjectile(direction);
        }
    }

    private void CreateProjectile(Vector3 direction)
    {
        GameObject projectile = GameManager.Instance.poolManager.Get(id);
        projectile.transform.position = transform.position;
        projectile.GetComponent<Skill>().Init(SkillType.Single, Attribute.Fire, damage, projectileCount, direction);
    }
}

