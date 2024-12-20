using UnityEngine;

public class Slime : MonoBehaviour, IHealth
{
    [SerializeField] private float healthMax;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = new HealthSystem(healthMax);
        healthSystem.OnDead += HealthSystem_OnDead;
    }
    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        GameManager.Instance.uiManager.Hide<SlimeHPUI>();
        //게임 종료 UI 불러오기
        Destroy(gameObject);
    }
    public void Damage()
    {
        healthSystem.Damage(5);
    }
    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
}