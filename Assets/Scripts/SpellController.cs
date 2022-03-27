using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class SpellController : SpellAPIs
{
    private int playerAgeGroup;
    private int score = 0;
    private int skipCount = 0; // Count how many times skip btn is pressed
    private TextAsset targetWordsList; // List of target words
    private List<int> wordListProbabilities; // Probability of selecting word from each list differs depending on age
    private List<string> askedTargetWords = new List<string>(); // Record asked words to avoid repetition

    public Text typedWord, messageText;
    public Button repeatButton, skipButton;
    public GameObject loader;

    // Start is called before the first frame update
    void Start() {
        SetupTTS();
        userId = PlayerPrefs.GetString("id");
        playerAgeGroup = PlayerPrefs.HasKey("ageGroup") ? PlayerPrefs.GetInt("ageGroup") : 0;
        PlayerPrefs.SetInt("bonusScore", 0); // Initialise score
        InitWordListProbabilities();
        GetRandomTargetWord(); 
    }

    // Method is called when repeat button pressed
    public void RepeatWord() {
        // Only allow 3 repeats per word
        if (repeatCount < 3) {
            SpeakWordWithTTS();
            // Increase remaining time by 3
            GameObject.Find("Remaining Time Text").GetComponent<Timer>().IncreaseTimeRemaining(3);
            repeatCount++;
            if (repeatCount == 3) 
                repeatButton.interactable = false;
        }
    }

    // Method is called when enter button pressed
    public void CheckSpelling() {
        StartCoroutine(RecordAttempt(typedWord.text.ToUpper())); // Record attempt in DB
        StartCoroutine(CheckSpellingAfterPhoneticDistance());
    }

    // Method is called when skip button pressed
    public void SkipWord() {
        // Only allow 2 skips
        if (skipCount < 2) {
            GetRandomTargetWord(true);
            skipCount++;
            typedWord.text = "";
            if (skipCount == 2)
                skipButton.interactable = false;
        }
    }

    // Setup TTS with params: language, pitch, speed
    private void SetupTTS() {
        TextToSpeech.instance.Setting("en-US", 1, (float)0.8);
    }

    // Set the probabilities of selecting a list to the probabilities for the player's age group
    private void InitWordListProbabilities() {  
        switch (playerAgeGroup) {
            case 1:
                wordListProbabilities = new List<int>{0, 1, 1}; // 33% list 0, 67% list 1
                break;
            case 2:
                wordListProbabilities = new List<int>{0, 1, 1, 2, 2, 2}; // 17% list 0, 33% list 1, 50% list 3
                break;
            case 3:
                wordListProbabilities = new List<int>{0, 1, 2, 2, 2, 3, 3, 3, 3, 3}; // 10% list 0, 10% list 1, 30% list 2, 50% list 3
                break;
            case 4:
                wordListProbabilities = new List<int>{0, 1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 4}; // 8% list 0, 8% list 1, 17% list 2, 25% list 3, 42% list 4
                break;
            default:
                wordListProbabilities = new List<int>{0}; // 100% chance to select list 0
                break;
        }
    }

    // Get a random word that hasn't been asked before and speak it using TTS
    private void GetRandomTargetWord(bool skipped = false) {
        // Record num repeats for the last word in DB
        if (repeatCount > 0) {
            StartCoroutine(UpdateNumRepeats());
            repeatCount = 0; // Reset repeat counter as new word being selected
        }

        // Record if last word has been skipped
        if (skipped) 
            StartCoroutine(UpdateSkipped()); // Update the skipped value in the DB

        string potentialTargetWord = GetPotentialRandomTargetWord();
        // While word has been asked pick a new one and if it hasn't been asked then this is new target word
        while (askedTargetWords.Contains(potentialTargetWord)) {
            string newPotentialTargetWord = GetPotentialRandomTargetWord();
            if (!askedTargetWords.Contains(newPotentialTargetWord)) {
                potentialTargetWord = newPotentialTargetWord;
                break;
            }
        }

        targetWord = potentialTargetWord;
        askedTargetWords.Add(targetWord);

        id = Guid.NewGuid().ToString(); // Generate a new unique identifier
        StartCoroutine(RecordAskedWord()); // Send the asked word to the DB

        SpeakWordWithTTS();
    }

    // Get a potential random target word (we will need to check if it has been asked already)
    private string GetPotentialRandomTargetWord() {
        GetWordsList();
        int numberOfWords = 94;
        switch (selectedWordList) {
            case 1:
                numberOfWords = 295;
                break;
            case 2:
                numberOfWords = 296;
                break;
            case 3:
                numberOfWords = 102;
                break;
            case 4:
                numberOfWords = 37;
                break;
            default:
                break;
        }
        string potentialTargetWord;
        int randomLineNumber = UnityEngine.Random.Range(1, numberOfWords+1); // Random lineNumber between 1 and num words in list
        using (StreamReader sr = new StreamReader(new MemoryStream(targetWordsList.bytes))) {
            for (int i = 1; i < randomLineNumber; i++)
                sr.ReadLine();
            potentialTargetWord = sr.ReadLine().Trim().ToUpper(); // Set targetWord to the randomly selected word
        }
        return potentialTargetWord;
    }

    // Select the words list using the probabilities for the player's age group
    private void GetWordsList() {
        int wordListIndex = UnityEngine.Random.Range(1, wordListProbabilities.Count-1); // Select a random element from list, naturally it will be in the given probabilities
        selectedWordList = wordListProbabilities[wordListIndex];
        targetWordsList = Resources.Load<TextAsset>($"targetWords_{selectedWordList.ToString()}"); // Load targetWords list
        Debug.Log($"Reading from file: targetWords_{selectedWordList.ToString()}.txt");
    }

    // Speak the target word using TTS and log the word
    private void SpeakWordWithTTS() {
        TextToSpeech.instance.StartSpeak(targetWord);
        Debug.Log("Target Word: " + targetWord);
    }

    // Wait for the RecordAttempt API to calculate phonetic distance or return an error then check spelling
    private IEnumerator CheckSpellingAfterPhoneticDistance() {
        while (!phoneticDistanceCalculatedOrError) {
            Debug.Log("Waiting for phonetic distance calculation OR server error");
            ChangeLoadingState(true);
            yield return new WaitForSeconds(0.1f);
        }
        ChangeLoadingState(false);
        if (phoneticDistance == 0 || targetWord.Equals(typedWord.text.ToUpper())) {
            AcceptSpellingAttempt();
        }
        else {
            RejectSpellingAttempt();
        }
    }

    private void ChangeLoadingState(bool isLoading) {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject button in buttons)
                button.GetComponent<Button>().interactable = !isLoading;
        loader.SetActive(isLoading);
    }

    // Display Incorrect error message
    private void RejectSpellingAttempt() {
        messageText.color = Color.red;
        messageText.text = "INCORRECT: Time to...Sound It Out and Try Again!";
    }

    // Increment score, display Correct message, clear typed word, reset repeat btn
    private void AcceptSpellingAttempt() {
        // If true then spelling correctly, if false then phonetic distance is 0 but spelled wrong
        bool correctSpelling = targetWord.Equals(typedWord.text.ToUpper());

        score = correctSpelling ? score+5 : score+2;
        PlayerPrefs.SetInt("bonusScore", score); // Store the Bonus Score
        messageText.color = correctSpelling ? Color.green : new Color(1.0f, 0.64f, 0.0f);
        messageText.text = correctSpelling ? "Correct, Well Done! Keep Going" : "Good Attempt at Sounding It Out! Correct answer was " + targetWord;
        typedWord.text = "";
        repeatButton.interactable = true;
        GetRandomTargetWord();
    }
}
