using UnityEngine;

public class Reposition : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area")) // Tag가 Area인지 확인 후 아니라면 return
            return;

        Vector3 playerPos = GameManager.Instance.player.transform.position; //Player위치
        Vector3 myPos = transform.position; // 타일맵 위치
        float diffX = Mathf.Abs(playerPos.x - myPos.x); //절대값으로 Player의 위치와 타일맵의 위치를 뺀다.
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        Vector3 playerDir = GameManager.Instance.playerMovement.moveDirection;
        float dirX = playerDir.x < 0 ? -1 : (playerDir.x > 0 ? 1 : 0); //플레이어의 X값이 +인지 -인지에 따라 -1 또는 1로
        float dirY = playerDir.y < 0 ? -1 : (playerDir.y > 0 ? 1 : 0);

        switch (transform.tag)
        {
            case "Ground":
                if (Mathf.Approximately(diffX, diffY))
                {
                    transform.Translate(Vector3.up * dirY * 80);
                    transform.Translate(Vector3.right * dirX * 80);
                }
                else if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 80);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 80);
                }
                break;
        }
    }
}
