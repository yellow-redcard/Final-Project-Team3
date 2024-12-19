using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownContactSlimeController : MonoBehaviour
{
    private GameObject getHealthSystemGameObject;
    private HealthSystem collidingHealthSystem;
    private string monsterTag = "Enemy";
    private bool isCollidingWithSlime;

    private void Start()
    {
        getHealthSystemGameObject = GameObject.FindGameObjectWithTag("Player");
        HealthSystem.TryGetHealthSystem(getHealthSystemGameObject, out HealthSystem healthSystem, true);
        collidingHealthSystem = healthSystem;
    }
    private void FixedUpdate()
    {
        if (isCollidingWithSlime)
        {
            ApplyHealthChange();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject receiver = collision.gameObject;
        collidingHealthSystem = collision.GetComponent<HealthSystem>();
        if (collidingHealthSystem != null)
        {
            isCollidingWithSlime = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isCollidingWithSlime = false;
    }
    private void ApplyHealthChange()
    {
        
    }
}
