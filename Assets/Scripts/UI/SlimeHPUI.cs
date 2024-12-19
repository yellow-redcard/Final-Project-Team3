using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeHPUI : MonoBehaviour
{
    public class HealthBarUI : MonoBehaviour
    {
        private GameObject getHealthSystem;
        private GameObject previousHealthSystem;
        [SerializeField] private Slider slider;


        private HealthSystem healthSystem;


        private void Start()
        {
            getHealthSystem = GameObject.FindGameObjectWithTag("Player");
            UpdateHealthSystem();
        }
        private void Update()
        {
            if (getHealthSystem != previousHealthSystem)
            {
                UpdateHealthSystem();
            }
        }
        private void UpdateHealthSystem()
        {
            getHealthSystem = GameObject.FindGameObjectWithTag("Player");
            if (HealthSystem.TryGetHealthSystem(getHealthSystem, out HealthSystem newHealthSystem))
            {
                healthSystem = newHealthSystem;
                previousHealthSystem = getHealthSystem;
                SetHealthSystem(healthSystem);
            }
            else
            {
                healthSystem = null;
                Debug.LogError("No HealthSystem found on the assigned GameObject!");
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
    }
}
