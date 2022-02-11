using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timeText;
    public GameObject timesUpPanel;
    private float timeRemaining = 10;

    // Update is called once per frame
    void Update() {
        if (timeRemaining > 0) {
            timeRemaining -= Time.deltaTime;
            timeText.text = ((int)timeRemaining).ToString();
        }
        else {
            timesUpPanel.SetActive(true);
        }
    }

    public void IncreaseTimeRemaining(int timeToIncreaseBy) {
        timeRemaining += timeToIncreaseBy;
    }
}
