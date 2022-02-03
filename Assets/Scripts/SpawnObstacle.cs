using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacle : MonoBehaviour
{
    public GameObject obstacle;
    public float timeBetweenSpawn;
    private float spawnTime;

    // Update is called once per frame
    void Update() {
        if (Time.time > spawnTime) {
            Spawn();
            spawnTime = Time.time + timeBetweenSpawn;
        }
    }

    void Spawn() {
        // Spawn obstacle with an X value of 10 passed the screen width
        float spawnX = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + 10;
        // Spawn obstacle at any Y value between 0 and the screen height
        float spawnY = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
 
        // Spawn the obstacle
        Vector2 spawnPosition = new Vector2(spawnX, spawnY);
        Instantiate(obstacle, spawnPosition, Quaternion.identity);
    }
}
