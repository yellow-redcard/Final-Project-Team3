using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeHPUI : UIBase
{
     private GameObject getHealthSystem;
    [SerializeField] private Slider slider;


    private HealthSystem currentHealthSystem;
    private void Start()
    {
        getHealthSystem = GameManager.Instance.slimeManager.currentSlime;
        if (HealthSystem.TryGetHealthSystem(getHealthSystem, out HealthSystem healthSystem))
        {
            Debug.Log("HealthSystem:"+ healthSystem);
            SetHealthSystem(healthSystem);
        }
        SetHealthSystem(healthSystem);
    }
    private void Update()
    {
        slider.value = GameManager.Instance.hpValue;
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
        GameManager.Instance.hpValue = slider.value;
    }
}
