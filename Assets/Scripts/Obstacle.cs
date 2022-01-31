using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Border") {
            Destroy(this.gameObject); // If obstacle hits border i.e. it's out of view, destroy it
        }
        else if (collision.tag == "Player") {
            Destroy(player.gameObject); // If player hits obstacle, destroy the player
        }
    }
}
