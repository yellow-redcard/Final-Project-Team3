using System;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    [SerializeField] public float speed;
    private TopDownController controller;
    private Rigidbody2D move_rb;
    private CharacterStatsHandler characterStatsHandler;
    private Vector2 moveDirection = Vector2.zero;

    public float maxHealth = 100f; // 최대 체력
    public float currentHealth; // 현재 체력
    public float baseSpeed = 5f; // 기본 이동 속도
    
    private void Awake()
    {
        controller = GetComponent<TopDownController>();
        move_rb = GetComponent<Rigidbody2D>();
        characterStatsHandler = GetComponent<CharacterStatsHandler>();
    }
    private void Start()
    {
        controller.OnMoveEvent += Move;
    }

    private void Move(Vector2 direction)
    {
        moveDirection = direction;
    }
    private void FixedUpdate()
    {
        ApplyMovement(moveDirection);
    }

    private void ApplyMovement(Vector2 direction)
    {
        direction = direction * characterStatsHandler.CurrentStat.speed;
        move_rb.velocity = direction;
    }
    public void AdjustSpeed(float rate)
    {
        baseSpeed += baseSpeed * rate; // 속도 증가
        Debug.Log($"[PlayerMovement] 새로운 속도: {baseSpeed}");
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth; // 체력 회복
        Debug.Log($"[PlayerMovement] 체력 회복 완료: {currentHealth}");
    }
}