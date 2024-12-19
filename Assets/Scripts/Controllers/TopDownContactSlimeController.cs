using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownContactSlimeController : MonoBehaviour
{
    [SerializeField] private GameObject getHealthSystemGameObject;
    private HealthSystem collidingTargetHealthSystem;
    private TopDownMovement collidingMovement;
    private void OnDamage()
    {

    }
}
