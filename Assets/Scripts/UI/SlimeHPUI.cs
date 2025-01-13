using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeHPUI : UIBase
{
    //private GameObject getHealthSystem;
    [SerializeField] private Slider slider;
    private HealthSystem healthUI;
    private void Start()
    {
        healthUI = GameManager.Instance.currentHealthSystem;
        SetHealthSystem(healthUI);
    }
    private void Update()
    {
        slider.value = GameManager.Instance.currentHealthSystem.GetHealthNormalized();
    }
    public void SetHealthSystem(HealthSystem healthSystem)
    {
        if (GameManager.Instance.currentHealthSystem != null)
        {
            GameManager.Instance.currentHealthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
        }
        GameManager.Instance.currentHealthSystem = healthSystem;

        UpdateHealthBar();

        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }
    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        //GameManager.Instance.currentHealthSystem.SetHealth(GameManager.Instance.currentHealth);
        GameManager.Instance.slimeManager.slimeDatas[GameManager.Instance.slimeManager.currentIndex].health = GameManager.Instance.currentHealthSystem.health;
    }
}
