using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform slime;  
    public float looksensivity = 0.125f;

    void LateUpdate()
    {
        Vector2 position = slime.position;
        Vector3 cameraPosition = Vector3.Lerp(transform.position, position, looksensivity);
        transform.position = cameraPosition;
    }
}
