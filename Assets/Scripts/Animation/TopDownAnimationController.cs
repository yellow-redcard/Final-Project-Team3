using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TopDownAnimationController : AnimationController
{
    // Animator.StringToHash�� ���� Animator ���� ��ȯ�� Ȱ��Ǵ� �κп� ���� ����ȭ�� ������ �� �ֽ��ϴ�.
    // StringToHash�� IsWalking�̶�� ���ڿ��� �Ϲ��� �Լ��� �ؽ��Լ��� ���� Ư���� ������ ��ȯ�մϴ�.
    // �� �ñ��Ͻôٸ� C# GetHashCode�� �˻��غ�����!
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsDie = Animator.StringToHash("IsDie");

    private readonly float magnituteThreshold = 0.5f;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        // �����ϰų� ������ �� �ִϸ��̼��� ���� �����ϵ��� ����
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