using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeHPUI : MonoBehaviour
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] private GameObject getHealthSystem;
        [SerializeField] private Slider slider;


        private HealthSystem healthSystem;


        private void Start()
        {
            if (HealthSystem.TryGetHealthSystem(getHealthSystem, out HealthSystem healthSystem))
            {
                SetHealthSystem(healthSystem);
            }
        }

        public void SetHealthSystem(HealthSystem healthSystem)
        {
            if (this.healthSystem != null)
            {
                this.healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
            }
            this.healthSystem = healthSystem;

            UpdateHealthBar();

            healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        }


        private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
        {
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            slider.value = healthSystem.GetHealthNormalized();
        }

        private void OnDamage()
        {
            HealthSystem.TryGetHealthSystem(getHealthSystem, out HealthSystem healthSystem, true);
            healthSystem.Damage(10);
        }
    }
}
