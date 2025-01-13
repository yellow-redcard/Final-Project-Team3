using UnityEngine;
using System.Collections;

public enum BossState
{
    Idle,          // 대기 상태
    ChargeReady,   // 돌진 준비
    ChargeAttack,  // 돌진 공격
    Cooldown       // 재사용 대기
}

public class BossMonsterFSM : MonoBehaviour
{
    public BossState currentState = BossState.Idle; // 초기 상태: Idle

    public float chargeSpeed = 5f; // 돌진 속도
    public float chargeDistance = 10f; // 돌진 거리
    public float cooldownTime = 2f; // 쿨다운 시간

    private Vector3 chargeDirection; // 돌진 방향
    private bool isCharging = false;

    private float cooldownTimer = 0f; // 쿨다운 타이머

    private void Update()
    {
        switch (currentState)
        {
            case BossState.Idle:
                UpdateIdleState();
                break;

            case BossState.ChargeReady:
                UpdateChargeReadyState();
                break;

            case BossState.ChargeAttack:
                UpdateChargeAttackState();
                break;

            case BossState.Cooldown:
                UpdateCooldownState();
                break;
        }
    }

    // ----------- 상태별 로직 -----------

    // Idle 상태: 플레이어와 거리 확인
    void UpdateIdleState()
    {
        if(GameManager.Instance.slimeManager.currentSlime != null)
        {
            if (Vector3.Distance(transform.position, GameManager.Instance.player.position) < chargeDistance)
            {
                // 돌진 준비 상태로 전환
                ChangeState(BossState.ChargeReady);
            }
        }
    }

    // 돌진 준비 상태
    void UpdateChargeReadyState()
    {
        if (!isCharging)
        {
            // 방향 설정
            chargeDirection = (GameManager.Instance.player.position - transform.position).normalized;

            // 애니메이션 트리거 또는 이펙트
            Debug.Log("돌진 준비!");

            // 잠깐의 준비 시간을 가진 후 공격으로 전환
            StartCoroutine(WaitAndCharge(1.0f)); // 1초 후 돌진 시작
            isCharging = true;
        }
    }

    // 돌진 상태
    void UpdateChargeAttackState()
    {
        if (!isCharging) return;

        // 돌진 이동
        transform.position += chargeDirection * chargeSpeed * Time.deltaTime;

        // 목표에 도달했거나 일정 거리 돌진 후 상태 전환
        if (Vector3.Distance(transform.position, GameManager.Instance.player.position) < 1.5f || chargeDistance <= 0f)
        {
            StopCharge();
        }

        chargeDistance -= chargeSpeed * Time.deltaTime; // 돌진 거리 감소
    }

    // 쿨다운 상태
    void UpdateCooldownState()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= cooldownTime)
        {
            // 쿨다운 종료 후 Idle 상태로 전환
            ChangeState(BossState.Idle);
            cooldownTimer = 0f;
        }
    }

    // ----------- 상태 전환 로직 -----------

    void ChangeState(BossState newState)
    {
        currentState = newState;
    }

    IEnumerator WaitAndCharge(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 돌진 상태로 전환
        ChangeState(BossState.ChargeAttack);
        isCharging = true;
    }

    void StopCharge()
    {
        isCharging = false;
        chargeDistance = 10f; // 거리 초기화
        ChangeState(BossState.Cooldown); // 쿨다운 상태로 전환
    }

    // 충돌 처리: 플레이어와 충돌 시 추가 행동
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어 명중!");
            StopCharge();
        }
    }
}
