using UnityEngine;

public class TopDownEnemyController : TopDownController
{
    protected Transform ClosestTarget { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Start()
    {
        ClosestTarget = GameManager.Instance.player;
    }

    protected virtual void FixedUpdate()
    {

    }

    protected float DistanceToTarget()
    {
        if (ClosestTarget == null)
        {
            UpdateTarget(); // 새로운 타겟 찾기
            if (ClosestTarget == null) return Mathf.Infinity; // 여전히 없으면 무한 거리 반환
        }
        return Vector3.Distance(transform.position, ClosestTarget.position);
    }

    private void UpdateTarget()
    {
        // 새로운 타겟을 태그로 찾아 설정
        GameObject targetObject = GameObject.FindGameObjectWithTag("Player");
        if (targetObject != null)
        {
            ClosestTarget = targetObject.transform;
        }
    }

    protected Vector2 DirectionToTarget()
    {
        return (ClosestTarget.position - transform.position).normalized;
    }
}
