using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgeSelector : MonoBehaviour
{
    public GameObject welcomePanel;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("playerAge")) {
            welcomePanel.SetActive(true);
        }
    }

    public void SetPlayerAge(int age) {
        PlayerPrefs.SetInt("playerAge", age);
        welcomePanel.SetActive(false);
    }
}
