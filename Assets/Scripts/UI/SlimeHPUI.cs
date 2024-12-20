using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeHPUI : UIBase
{
    [SerializeField] private GameObject getHealthSystem;
    [SerializeField] private GameObject previousHealthSystem;
    [SerializeField] private Slider slider;

    private HealthSystem currentHealthSystem;
    private void Start()
    {
        getHealthSystem = GameObject.FindGameObjectWithTag("Player");
        previousHealthSystem = GameObject.FindGameObjectWithTag("Player");
        if (HealthSystem.TryGetHealthSystem(getHealthSystem, out HealthSystem healthSystem))
        {
            SetHealthSystem(healthSystem);
        }
    }
    private void Update()
    {
        getHealthSystem = GameObject.FindGameObjectWithTag("Player");
        if (getHealthSystem != previousHealthSystem)
        {
            UpdateHealthSystem();
        }
    }
    private void UpdateHealthSystem()
    {
        if (HealthSystem.TryGetHealthSystem(getHealthSystem, out HealthSystem newHealthSystem))
        {
            currentHealthSystem = newHealthSystem;
            previousHealthSystem = getHealthSystem;
        }
        else
        {
            currentHealthSystem = null;
            Debug.LogError("No HealthSystem found on the assigned GameObject!");
        }
    }
    public void SetHealthSystem(HealthSystem healthSystem)
    {
        if (this.currentHealthSystem != null)
        {
            this.currentHealthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
        }
        this.currentHealthSystem = healthSystem;

        UpdateHealthBar();

        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }
    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        Debug.Log($"슬라임 체력바 {slider.value}");
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        slider.value = currentHealthSystem.GetHealthNormalized();
    }
}