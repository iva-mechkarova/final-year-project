using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgeSelector : MonoBehaviour
{
    public GameObject welcomePanel;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("ageGroup")) {
            welcomePanel.SetActive(true);
        }
        else {
            Debug.Log($"Age group: {PlayerPrefs.GetInt("ageGroup").ToString()}");
        }
    }

    public void SetPlayerAge(int age) {
        PlayerPrefs.SetInt("ageGroup", age);
        welcomePanel.SetActive(false);
    }
}
