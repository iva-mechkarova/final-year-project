using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerSpeed;
    private Rigidbody2D player;

    // Start is called before the first frame update
    void Start() {
        // Spawn the player in the same position regardless of screen size i.e. responsive
        float spawnX = 1 - Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
        transform.position = new Vector2(spawnX, 0);
        
        player = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update() {
        // Check if at least one finger touching screen
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase) {
            case TouchPhase.Began:
                MovePlayer(touch.position.y);
                break;
            case TouchPhase.Ended:
                StopPlayer();
                break;
            }
        }
        // Check if left mouse button is clicked
        else if (Input.GetMouseButtonDown(0)) {
            MovePlayer(Input.mousePosition.y);
        }
        // Check if left mouse button is released
        else if (Input.GetMouseButtonUp(0)) {
            StopPlayer();
        }
    }

    private void MovePlayer(float yPos) {
        // If touch pos <= half the screen height then move down, otherwise move up
        // Using <= instead of < as we don't want a pos that won't move the player i.e. exactly Screen.height/2
        player.velocity = (yPos <= Screen.height / 2) ? new Vector2(0, -playerSpeed) : new Vector2(0, playerSpeed);
    }

    private void StopPlayer() {
        player.velocity = new Vector2(0, 0);
    }
}
