using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitializeGame : MonoBehaviour
{
    public GameObject welcomePanel;

    [SerializeField]
    private Button[] ageButtons;

    // Start is called before the first frame update
    void Start() {
        // If no ageGroup key, then this is first time opening app
        if (!PlayerPrefs.HasKey("ageGroup")) {
            welcomePanel.SetActive(true);
            PlayerPrefs.SetInt("selectedCharacter", 0); // Init selectedCharacter
            PlayerPrefs.SetInt("fish-0", 1); // Set starter fish as unlocked
        }
        else {
            Debug.Log($"Age group: {PlayerPrefs.GetInt("ageGroup").ToString()}");
            Debug.Log($"Selected Character: {PlayerPrefs.GetInt("selectedCharacter").ToString()}");
        }
    }

    public void SetPlayerAge(int age) {
        PlayerPrefs.SetInt("ageGroup", age);
        welcomePanel.SetActive(false);
    }

    public void ConsentToggled() {
        foreach (Button button in ageButtons) {
            button.interactable = !button.interactable;
        }
    }
}
