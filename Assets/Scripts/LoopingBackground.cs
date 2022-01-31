using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    public GameObject cam;
    public float parallaxSpeed;
    private float length;
    private float startPos;

    void Start() {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate() {
        float temp = (cam.transform.position.x * (1 - parallaxSpeed));

        // Create parallax effect by moving background at inputted speed
        float distance = (cam.transform.position.x * parallaxSpeed);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        // Ensure that the background loops back around
        if (temp > startPos + length) 
            startPos += length;
        else if (temp < startPos - length)
            startPos -= length;   
    }
}
