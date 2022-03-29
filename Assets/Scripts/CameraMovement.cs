using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraSpeed;

    // Update is called once per frame
    void Update()
    {
        // Move camera on x-axis at specified speed
        transform.position += new Vector3(cameraSpeed * Time.deltaTime, 0, 0);
    }
}
