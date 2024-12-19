using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnHealthMaxChanged;
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDead;

    private float healthMax;
    private float health;

    public HealthSystem(float healthMax)
    {
        this.healthMax = healthMax;
        health = healthMax;
    }
    public float GetHealthNormalized()
    {
        return health / healthMax;
    }
    public void Damage(float amount)
    {
        health -= amount;
        if (health < 0)
        {
            health = 0;
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
    public bool IsDead()
    {
        return health <= 0;
    }
    public void Heal(float amount)
    {
        health += amount;
        if (health > healthMax)
        {
            health = healthMax;
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnHealed?.Invoke(this, EventArgs.Empty);
    }
    public void SetHealth(float health)
    {
        if (health > healthMax)
        {
            health = healthMax;
        }
        if (health < 0)
        {
            health = 0;
        }
        this.health = health;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);

        if (health <= 0)
        {
            Die();
        }
    }
    public static bool TryGetHealthSystem(GameObject getHealthSystemGameObject, out HealthSystem healthSystem, bool logErrors = false)
    {
        healthSystem = null;

        if (getHealthSystemGameObject != null)
        {
            if (getHealthSystemGameObject.TryGetComponent(out IHealth getHealthSystem))
            {
                healthSystem = getHealthSystem.GetHealthSystem();
                if (healthSystem != null)
                {
                    return true;
                }
                else
                {
                    if (logErrors)
                    {
                        Debug.LogError($"healthSystem이 null입니다. 작업 순서에 문제가 있는 것 같습니다.");
                    }
                    return false;
                }
            }
            else
            {
                if (logErrors)
                {
                    Debug.LogError($"'{getHealthSystemGameObject}'에 interface IHealth를 구현하세요.");
                }
                return false;
            }
        }
        else
        {
            if (logErrors)
            {
                Debug.LogError($"'getHealthSystemGameObject'를 세팅하세요.");
            }
            return false;
        }
    }
}
