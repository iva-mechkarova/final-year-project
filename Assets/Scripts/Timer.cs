using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timeText;
    private float timeRemaining = 100;

    // Update is called once per frame
    void Update() {
        timeRemaining -= Time.deltaTime;
        timeText.text = ((int)timeRemaining).ToString();
    }
}
