using UnityEngine;
using UnityEngine.InputSystem;

public class SlimeInputController : TopDownController
{
    public void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>().normalized;
        CallMoveEvent(moveInput);
    }
}
