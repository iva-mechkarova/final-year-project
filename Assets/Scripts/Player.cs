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
