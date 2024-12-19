using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour, IHealth
{
    [SerializeField] private float healthMax;
    [SerializeField] private float healthAmount;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = new HealthSystem(healthMax);

        if (healthAmount != 0)
        {
            healthSystem.SetHealth(healthAmount);
        }
    }
    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
}