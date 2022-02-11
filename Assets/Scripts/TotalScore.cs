using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalScore : MonoBehaviour
{
    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("totalScore"))
            scoreText.text = PlayerPrefs.GetInt("totalScore").ToString();
    }
}
