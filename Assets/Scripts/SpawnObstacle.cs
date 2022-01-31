using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacle : MonoBehaviour
{
    public GameObject obstacle;
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;
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
        float spawnX = Random.Range(minX, maxX);
        float spawnY = Random.Range(minY, maxY);

        // Spawn an obstacle in random position, where x and y are between maxX and minX and maxY and minY respectively
        Instantiate(obstacle, transform.position + new Vector3(spawnX, spawnY, 0), transform.rotation);
    }
}
