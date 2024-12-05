using UnityEngine;
using UnityEngine.InputSystem;

public class SlimeInputController : TopDownController
{
    public Animator moveAnimator;
    public void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>().normalized;
        CallMoveEvent(moveInput);
        MoveAnimation(value);
    }
    private void MoveAnimation(InputValue value)
    {
        Vector2 move = value.Get<Vector2>();
        if (move.x > 0)
        {
            moveAnimator.Play("MoveRight");
        }
        else if (move.x < 0)
        {
            moveAnimator.Play("MoveLeft");
        }
        else if (move.y > 0)
        {
            moveAnimator.Play("MoveUp");
        }
        else if (move.y < 0)
        {
            moveAnimator.Play("MoveDown");
        }
        else
        {
            moveAnimator.Play("Idle");
        }
    }
}

