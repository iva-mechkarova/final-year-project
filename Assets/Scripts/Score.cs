using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    private float score;

    // Update is called once per frame
    void Update()
    {
        // If the Player object is not null i.e. the player has not hit an obstacle
        if (GameObject.FindGameObjectWithTag("Player") != null) {
            score += Time.deltaTime; // Increase the score by 1 every second
            scoreText.text = ((int)score).ToString();
        }
    }
}
