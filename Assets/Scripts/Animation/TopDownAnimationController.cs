using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TopDownAnimationController : AnimationController
{
    // Animator.StringToHash를 통해 Animator 변수 전환에 활용되는 부분에 대한 최적화를 진행할 수 있습니다.
    // StringToHash는 IsWalking이라는 문자열을 일방향 함수인 해쉬함수를 통해 특정한 값으로 변환합니다.
    // 더 궁금하시다면 C# GetHashCode를 검색해보세요!
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsDie = Animator.StringToHash("IsDie");

    private readonly float magnituteThreshold = 0.5f;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        // 공격하거나 움직일 때 애니메이션이 같이 반응하도록 구독
        controller.OnMoveEvent += Move;
    }

    private void Move(Vector2 obj)
    {
        animator.SetBool(IsWalking, obj.magnitude > magnituteThreshold);
    }

    private void Die()
    {
        animator.SetBool(IsDie, true);
    }

    private void InvincibilityEnd()
    {
        animator.SetBool(IsDie, false);
    }
}