using System;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    private TopDownController controller;
    private Rigidbody2D move_rb;
    private CharacterStatsHandler characterStatsHandler;
    private Vector2 moveDirection = Vector2.zero;
    private Collider2D _collider;
    
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
        // 속도 증가
    }

    public void RestoreHealth()
    {
        //체력 회복
    }
}