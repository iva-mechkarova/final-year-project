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
    private string targetWord = "Television"; // Initialise to Television in case we fail to fetch a random word
    private int score = 0;
    private int repeatCount = 0; // Count how many times repeat btn is pressed

    public Text typedWord, messageText;
    public Button repeatButton;

    // Start is called before the first frame update
    void Start() {
        SetupTTS(LANG_CODE);
        GetRandomTargetWord(); 
    }

    // Method is called when repeat button pressed
    public void RepeatWord() {
        SpeakWordWithTTS();
        // Increase remaining time by 10
        GameObject.Find("Remaining Time Text").GetComponent<Timer>().IncreaseTimeRemaining(10);
        // Only allow 3 repeats per word
        if (repeatCount < 2)
            repeatCount++;
        else
            repeatButton.interactable = false;
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

    // Setup TTS with params: language, pitch, speed
    private void SetupTTS(string code) {
        TextToSpeech.instance.Setting(code, 1, 1);
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
        score++;
        messageText.color = Color.green;
        messageText.text = "Good job! Try the next word";
        typedWord.text = "";
        repeatCount = 0;
        repeatButton.interactable = true;
    }

    // Get a random word from the list and speak it using TTS
    private void GetRandomTargetWord() {
        TextAsset words = Resources.Load<TextAsset>("targetWords"); // Load targetWords list
        int lineNumber = Random.Range(1, 11); // Random lineNumber between 1 and 10 (num words in list)
        using (StreamReader sr = new StreamReader(new MemoryStream(words.bytes))) {
            for (int i = 1; i < lineNumber; i++)
                sr.ReadLine();
            targetWord = sr.ReadLine(); // Set targetWord to the randomly selected word
        }
        SpeakWordWithTTS();
    }  
}
