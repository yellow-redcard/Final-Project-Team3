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
        //getHealthSystem = GameManager.Instance.slimeManager.currentSlime;
        /*
        if (HealthSystem.TryGetHealthSystem(getHealthSystem, out HealthSystem healthSystem))
        {
            Debug.Log("HealthSystem:"+ healthSystem);
            SetHealthSystem(healthSystem);
        }
        */
        //sethealthsystem(gam);
        healthUI = GameManager.Instance.currentHealthSystem;
        //SetHealthSystem(healthUI);
    }
    private void Update()
    {
        slider.value = GameManager.Instance.hpValue;
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
        slider.value = GameManager.Instance.currentHealthSystem.GetHealthNormalized();
        GameManager.Instance.currentHealth = GameManager.Instance.currentHealthSystem.health;
        GameManager.Instance.slimeManager.slimeDatas[GameManager.Instance.slimeManager.currentIndex].health = GameManager.Instance.currentHealth;
    }
}
