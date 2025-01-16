using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    // 골드 데이터를 관리하는 변수
    public static int goldCount = 0;


    // 골드가 플레이어에게 빨려 들어가는 거리와 속도
    public float attractDistance = 5f; // 빨려 들어가기 시작하는 거리
    public float moveSpeed = 10f;       // 골드 이동 속도

    // 골드가 빨려 들어가는 중인지 확인
    private bool isAttracting = false;
    private float distanceToPlayer;

    void Update()
    {
        if (GameManager.Instance.slimeManager.currentSlime != null)
        {
            // 플레이어와 골드 사이의 거리 계산
            distanceToPlayer = Vector3.Distance(transform.position, GameManager.Instance.player.position);
        }
        // 플레이어가 일정 거리 내에 있으면 빨려 들어가도록 설정
        if (distanceToPlayer <= attractDistance)
        {
            isAttracting = true;
        }

        // 골드가 플레이어를 향해 움직이도록 처리
        if (isAttracting)
        {
            // 플레이어 방향으로 이동
            if (GameManager.Instance.slimeManager.currentSlime != null)
            {
                Vector3 direction = (GameManager.Instance.player.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
            // 플레이어에 도달하면 골드 획득 처리
            if (distanceToPlayer <= 0.5f) // 0.5f는 도달했다고 간주하는 거리
            {
                CollectGold();
            }
        }
    }

    // 골드 획득 처리
    private void CollectGold()
    {
        // 골드 데이터 증가
        goldCount += 1;

        // 골드 오브젝트 제거
        Destroy(gameObject);
    }

    // 충돌 처리 (예비 처리, 직접 닿을 경우에도 획득 가능)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollectGold();
        }
    }
}
