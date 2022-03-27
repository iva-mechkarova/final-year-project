using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timeText;
    public GameObject timesUpPanel;
    public Button keyboardButton;
    private float timeRemaining = 100;

    // Update is called once per frame
    void Update() {
        if (timeRemaining > 0) {
            // If keyboard button is not interactable then pause timer as waiting for server response
            if (keyboardButton.interactable) {
                timeRemaining -= Time.deltaTime;
                timeText.text = ((int)timeRemaining).ToString();
            }
        }
        else {
            timesUpPanel.SetActive(true);
        }
    }

    public void IncreaseTimeRemaining(int timeToIncreaseBy) {
        timeRemaining += timeToIncreaseBy;
    }
}
