using System.Collections;
using UnityEngine;

public class Slime : MonoBehaviour, IHealth
{
    [SerializeField] private float healthMax;

    private HealthSystem healthSystem;
    private int AnimationIndex;
    public Animator deadAnimator;

    private void Start()
    {
        healthSystem = gameObject.GetComponent<HealthSystem>();
        healthSystem.Initialize(healthMax);
        GameManager.Instance.currentHealthSystem = healthSystem;
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
    public void BossDamage()
    {
        healthSystem.Damage(50);
    }
    void OnDead()
    {
        AnimationIndex = GameManager.Instance.slimeManager.currentIndex;
        deadAnimator = GameManager.Instance.slimeManager.slimeBodies[AnimationIndex].GetComponent<Animator>();
        StartCoroutine(OnDeadComplete());
    }
    private IEnumerator OnDeadComplete()
    {
        deadAnimator.Play("Dead");
        yield return new WaitForSeconds(deadAnimator.GetCurrentAnimatorStateInfo(0).length);
        Time.timeScale = 0f;
        //GameManager.Instance.uiManager.CloseUI();
        Destroy(gameObject);
        GameManager.Instance.uiManager.Show<GameOverUI>();
        //scene 전환
    }
   
    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
}