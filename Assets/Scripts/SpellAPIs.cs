using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class SpellAPIs : MonoBehaviour {
    // All fields protected as SpellController script must be able to access
    protected string id;
    protected string userId;
    protected string targetWord;
    protected int repeatCount = 0; // Count how many times repeat btn is pressed
    protected int selectedWordList = 0; // Stores which list we are reading from (asc difficulty of 0 - 4)
    protected int phoneticDistance = 4; // Stores the most recent attempt's distance from target word (0 - 4, where 0 is identical)
    protected bool phoneticDistanceCalculatedOrError = false;

    protected IEnumerator RecordAskedWord() {
        WWWForm form = new WWWForm();
        // Send the necessary attributes to the API
        form.AddField("id", id);
        form.AddField("userId", userId);
        form.AddField("targetWord", targetWord);
        form.AddField("difficulty", selectedWordList);

        // Call the Web API that records the asked word to the MySQL DB
        using (UnityWebRequest www = UnityWebRequest.Post("http://52.212.158.172/sounditout/RecordAskedWord.php", form)) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log(www.error);
            else
                Debug.Log("Asked word recorded successfully!");
        }
    }

    protected IEnumerator UpdateNumRepeats() {
        WWWForm form = new WWWForm();
        // Send the necessary attributes to the API
        form.AddField("id", id);
        form.AddField("numRepeats", repeatCount);

        // Call the Web API that updates the num_repeats value in the DB for the given id
        using (UnityWebRequest www = UnityWebRequest.Post("http://52.212.158.172/sounditout/UpdateNumRepeats.php", form)) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log(www.error);
            else
                Debug.Log("Num repeats updated successfully!");
        }
    }
    
    protected IEnumerator UpdateSkipped() {
        WWWForm form = new WWWForm();
        // Send the necessary attributes to the API
        form.AddField("id", id);

        // Call the Web API that updates the skipped value to TRUE in the DB for the given id
        using (UnityWebRequest www = UnityWebRequest.Post("http://52.212.158.172/sounditout/UpdateSkipped.php", form)) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log(www.error);
            else
                Debug.Log("Skipped updated successfully!");
        }
    }
    
    protected IEnumerator RecordAttempt(string attempt) {
        phoneticDistance = 4; // Reset phonetic distance
        phoneticDistanceCalculatedOrError = false;

        WWWForm form = new WWWForm();
        // Send the necessary attributes to the API
        string attemptId = Guid.NewGuid().ToString();
        form.AddField("id", attemptId);
        form.AddField("wordId", id);
        form.AddField("attempt", attempt);

        // Call the Web API that updates the skipped value to TRUE in the DB for the given id
        using (UnityWebRequest www = UnityWebRequest.Post("http://52.212.158.172/sounditout/RecordAttempt.php", form)) {
            www.timeout = 4; // Wait up to 4 seconds
            yield return www.SendWebRequest();
          
            if (www.result != UnityWebRequest.Result.Success) {
                phoneticDistanceCalculatedOrError = true;
                if (www.error == null)
                    Debug.Log("RecordAttempt timed out - server is down or slow");
                else 
                    Debug.Log(www.error);
            }
            else {
                Debug.Log("Recorded attempt successfully");
                string[] soundexCodes = www.downloadHandler.text.Split(' ');
                CalculatePhoneticDistance(soundexCodes);
                phoneticDistanceCalculatedOrError = true;
                StartCoroutine(UpdatePhoneticDistance(attemptId));
            }
        }
    }

    protected IEnumerator UpdatePhoneticDistance(string attemptId) {
        WWWForm form = new WWWForm();
        // Send the necessary attributes to the API
        form.AddField("id", attemptId);
        form.AddField("phoneticDistance", phoneticDistance);

        // Call the Web API that updates the skipped value to TRUE in the DB for the given id
        using (UnityWebRequest www = UnityWebRequest.Post("http://52.212.158.172/sounditout/UpdatePhoneticDistance.php", form)) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log(www.error);
            else
                Debug.Log("Phonetic distance updated successfully!");
        }
    }

    // Method to calculate the phonetic distance using two soundex codes
    private void CalculatePhoneticDistance(string[] soundexCodes) {
        phoneticDistance = 4;
        for (int i=0; i<4; i++) {
            if (soundexCodes[0][i]==soundexCodes[1][i])
                phoneticDistance--;
        }
    }
}
