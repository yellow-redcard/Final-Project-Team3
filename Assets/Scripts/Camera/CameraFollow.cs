using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform slime;
    public float cameraPositionZ = -10f;

    void LateUpdate()
    {
        Vector3 position = slime.position;
        Vector3 cameraPosition = new Vector3(position.x, position.y, cameraPositionZ);
        transform.position = cameraPosition;
    }
}