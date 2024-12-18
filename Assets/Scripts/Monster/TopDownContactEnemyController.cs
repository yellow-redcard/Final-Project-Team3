using UnityEngine;

public class TopDownContactEnemyController : TopDownEnemyController
{
    [SerializeField][Range(0f, 100f)] private float followRange;
    [SerializeField] private string targetTag = "Player";
    private bool isCollidingWithTarget;

    [SerializeField] private Transform characterTransform;

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Vector2 direction = Vector2.zero;
        if(DistanceToTarget() < followRange)
        {
            direction = DirectionToTarget();
        }

        CallMoveEvent(direction);
        Rotate(direction);
    }

    private void Rotate(Vector2 direction)
    {
        if (direction == Vector2.zero) return; // 방향이 없으면 회전하지 않음

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Transform의 로컬 스케일 변경으로 캐릭터 방향 조정
        Vector3 localScale = characterTransform.localScale;
        localScale.x = Mathf.Abs(localScale.x) * (Mathf.Abs(rotZ) > 90f ? 1 : -1);
        characterTransform.localScale = localScale;
    }
}