using UnityEngine;

public class TileMapManager : MonoBehaviour
{
    public Transform map; // 맵의 부모 객체 (Hierarchy에서 설정)
    public Transform player; // 플레이어 Transform (GameManager에서 전달받음)

    private Vector3 lastPlayerPosition; // 플레이어의 이전 위치

    public void Init(Transform playerTransform)
    {
        // 플레이어 Transform 저장 및 초기 위치 설정
        player = playerTransform;
        lastPlayerPosition = player.position;
    }

    void Update()
    {
        if (player == null || map == null) return;

        // 플레이어 이동량 계산
        Vector3 offset = player.position - lastPlayerPosition;

        // 맵을 플레이어 이동량만큼 이동
        map.position += offset;

        // 이전 위치 갱신
        lastPlayerPosition = player.position;
    }
}
