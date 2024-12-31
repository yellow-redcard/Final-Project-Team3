using UnityEngine;

public class Slime : MonoBehaviour, IHealth
{
    [SerializeField] private float healthMax;

    private HealthSystem healthSystem;

    public Animator deadAnimator;

    private void Awake()
    {
        healthSystem = new HealthSystem(healthMax);
        healthSystem.OnDead += HealthSystem_OnDead;
    }
    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        OnDead();
        //게임 종료 UI 불러오기
    }
    public void Damage()
    {
        healthSystem.Damage(20);
    }
    void OnDead()
    {
        deadAnimator.Play("Dead");
    }
    void OnDeadComplete()
    {
        Time.timeScale = 0f;
        Destroy(gameObject);
    }
    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
}