using System;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    [SerializeField] public float speed;
    private TopDownController controller;
    private Rigidbody2D move_rb;
    private Vector2 moveDirection = Vector2.zero;

    private void Awake()
    {
        controller = GetComponent<TopDownController>();
        move_rb = GetComponent<Rigidbody2D>();
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
        direction = direction * speed;
        move_rb.velocity = direction;
    }
}