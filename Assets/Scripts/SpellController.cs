using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class SpellController : MonoBehaviour
{
    private const string LANG_CODE = "en-US"; // TTS language code
    private int playerAgeGroup;
    private string targetWord;
    private int score = 0;
    private int repeatCount = 0; // Count how many times repeat btn is pressed
    private int skipCount = 0;
    private TextAsset targetWords = Resources.Load<TextAsset>("targetWords_0");

    public Text typedWord, messageText;
    public Button repeatButton, skipButton;

    // Start is called before the first frame update
    void Start() {
        SetupTTS(LANG_CODE);
        playerAgeGroup = PlayerPrefs.GetInt("ageGroup");
        PlayerPrefs.SetInt("bonusScore", 0); // Initialise score
        GetWordsList();
        GetRandomTargetWord(); 
    }

    // Method is called when repeat button pressed
    public void RepeatWord() {
        // Only allow 3 repeats per word
        if (repeatCount < 3) {
            SpeakWordWithTTS();
            // Increase remaining time by 10
            GameObject.Find("Remaining Time Text").GetComponent<Timer>().IncreaseTimeRemaining(10);
            repeatCount++;
            if (repeatCount == 3) 
                repeatButton.interactable = false;
        }
    }

    // Method is called when enter button pressed
    public void CheckSpelling() {
        messageText.gameObject.SetActive(true);

        if (targetWord.ToUpper().Equals(typedWord.text.ToUpper())) {
            AcceptSpellingAttempt();
            GetRandomTargetWord();
        }
        else {
            RejectSpellingAttempt();
        }
    }

    // Method is called when skip button pressed
    public void SkipWord() {
        // Only allow 2 skips
        if (skipCount < 2) {
            GetRandomTargetWord();
            skipCount++;
            if (skipCount == 2)
                skipButton.interactable = false;
        }
    }

    // Setup TTS with params: language, pitch, speed
    private void SetupTTS(string code) {
        TextToSpeech.instance.Setting(code, 1, 1);
    }

    // Get the words list with the relevant difficulty
    private void GetWordsList() {
        targetWords = Resources.Load<TextAsset>($"targetWords_{playerAgeGroup.ToString()}"); // Load targetWords list
        Debug.Log($"Reading from file: targetWords_{playerAgeGroup.ToString()}.txt");
    }

    // Speak the target word using TTS and log the word
    private void SpeakWordWithTTS() {
        TextToSpeech.instance.StartSpeak(targetWord);
        Debug.Log("Target Word: " + targetWord);
    }

    // Display Incorrect error message
    private void RejectSpellingAttempt() {
        messageText.color = Color.red;
        messageText.text = "INCORRECT: Time to...Sound It Out and Try Again!";
    }

    // Increment score, display Correct message, clear typed word, reset repeat btn
    private void AcceptSpellingAttempt() {
        score += 10;
        PlayerPrefs.SetInt("bonusScore", score); // Store the Bonus Score
        messageText.color = Color.green;
        messageText.text = "Good job! Try the next word";
        typedWord.text = "";
        repeatCount = 0;
        repeatButton.interactable = true;
    }

    // Get a random word from the list and speak it using TTS
    private void GetRandomTargetWord() {
        int numberOfWords = 93;
        switch (playerAgeGroup) {
            case 1:
                numberOfWords = 388;
                break;
            case 2:
                numberOfWords = 683;
                break;
            case 3:
                numberOfWords = 784;
                break;
            case 4:
                numberOfWords = 820;
                break;
            default:
                break;
        }

        int randomLineNumber = Random.Range(1, numberOfWords+1); // Random lineNumber between 1 and num words in list
        using (StreamReader sr = new StreamReader(new MemoryStream(targetWords.bytes))) {
            for (int i = 1; i < randomLineNumber; i++)
                sr.ReadLine();
            targetWord = sr.ReadLine(); // Set targetWord to the randomly selected word
        }
        SpeakWordWithTTS();
    }
}
