using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    // 골드 데이터를 관리하는 변수
    public static int goldCount = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트가 플레이어인지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            // 골드 데이터 증가
            goldCount += 1;

            // 골드 오브젝트 제거
            Destroy(gameObject);

            // 디버그 메시지 출력 (테스트용)
        }
    }
}
