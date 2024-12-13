using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform slime;
    public float cameraPositionZ = -10f;

    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        slime = GameManager.Instance.slimeManager.currentSlime.transform;
        Vector3 position = slime.position;
        Vector3 cameraPosition = new Vector3(position.x, position.y, cameraPositionZ);
        transform.position = cameraPosition;
        
    }
}