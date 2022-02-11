using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScores : MonoBehaviour
{
    public Text mainScoreText, bonusScoreText, totalScoreText;

    // Start is called before the first frame update
    void Start() {
        int mainScore = PlayerPrefs.HasKey("mainScore") ? PlayerPrefs.GetInt("mainScore") : 0; // Might have gone straight to Spell Mode
        int bonusScore = PlayerPrefs.GetInt("bonusScore");
        int totalScore = mainScore + bonusScore;
        SaveTotalScore(totalScore);

        mainScoreText.text = mainScore.ToString();
        bonusScoreText.text = bonusScore.ToString();
        totalScoreText.text = totalScore.ToString();
    }

    private void SaveTotalScore(int totalScore) {
        // Check if this is the first time score will be saved
        if (PlayerPrefs.HasKey("totalScore")) {
            int currentTotal = PlayerPrefs.GetInt("totalScore");
            PlayerPrefs.SetInt("totalScore", currentTotal + totalScore); // Add the score to the existing score
        }
        else {
            PlayerPrefs.SetInt("totalScore", totalScore); // Save the score from this game as the total
        }
    }
}
