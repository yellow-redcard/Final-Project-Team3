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
        float dirX = playerPos.x - myPos.x;
        float dirY = playerPos.y - myPos.y;

        float diffx = Mathf.Abs(dirX);
        float diffy = Mathf.Abs(dirY);

        dirX = dirX > 0 ? 1 : -1;
        dirY = dirY > 0 ? 1 : -1;

        switch (transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                {
                    transform.Translate(dirX * 80, 0, 0);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(0, dirY * 80, 0);
                }
                else
                {
                    transform.Translate(dirX * 80, dirY * 80, 0);
                }
                break;
        }
    }
}
