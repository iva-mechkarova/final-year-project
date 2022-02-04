using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerSpeed;
    private Rigidbody2D player;
    private Vector2 playerDirection;

    // Start is called before the first frame update
    void Start() {
        // Spawn the player in the same position regardless of screen size i.e. responsive
        float spawnX = 1 - Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
        transform.position = new Vector2(spawnX, 0);
        
        player = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update() {
        float verticalDirection = Input.GetAxisRaw("Vertical"); // Player can only move vertically
        playerDirection = new Vector2(0, verticalDirection).normalized;
    }

    void FixedUpdate() {
        player.velocity = new Vector2(0, playerDirection.y * playerSpeed); // Move player vertically
    }
}
