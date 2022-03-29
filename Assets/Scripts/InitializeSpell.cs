using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitializeSpell : MonoBehaviour
{
    public GameObject readyPanel, spellPanel, gameScoreContainer, totalScoreContainer;
    public Text bonusStarsText;

    // Start is called before the first frame update
    void Start() {
        // If main game has been played, else went directly to Spell Mode
        if (PlayerPrefs.HasKey("mainScore") && PlayerPrefs.GetInt("mainScore") != 0) {
            Debug.Log("Score: " + PlayerPrefs.GetInt("mainScore").ToString());
            readyPanel.SetActive(false);
            spellPanel.SetActive(true);
        }
        else {
            readyPanel.SetActive(true);
            spellPanel.SetActive(false);
            bonusStarsText.text = "Stars:";
            gameScoreContainer.SetActive(false);
            totalScoreContainer.SetActive(false);
        }
    }

    public void ReadyButtonPressed() {
        readyPanel.SetActive(false);
        spellPanel.SetActive(true);
    }

    // OnApplicationQuit is called when app is quit/shutdown
    void OnApplicationQuit() {
        // Reset scores if application is quit
        PlayerPrefs.SetInt("mainScore", 0);
        PlayerPrefs.SetInt("bonusScore", 0);
    }
}
