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
    protected int phoneticDistance = 0; // Stores the most recent attempt's distance from target word (0 - 4, where 0 is identical)

    protected IEnumerator RecordAskedWord() {
        WWWForm form = new WWWForm();
        // Send the necessary attributes to the API
        form.AddField("id", id);
        form.AddField("userId", userId);
        form.AddField("targetWord", targetWord);
        form.AddField("difficulty", selectedWordList);

        // Call the Web API that records the asked word to the MySQL DB
        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.122/sounditout/RecordAskedWord.php", form)) {
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
        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.122/sounditout/UpdateNumRepeats.php", form)) {
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
        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.122/sounditout/UpdateSkipped.php", form)) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log(www.error);
            else
                Debug.Log("Skipped updated successfully!");
        }
    }
    
    protected IEnumerator RecordAttempt(string attempt) {
        WWWForm form = new WWWForm();
        // Send the necessary attributes to the API
        string attemptId = Guid.NewGuid().ToString();
        form.AddField("id", attemptId);
        form.AddField("wordId", id);
        form.AddField("attempt", attempt);

        // Call the Web API that updates the skipped value to TRUE in the DB for the given id
        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.122/sounditout/RecordAttempt.php", form)) {
            yield return www.SendWebRequest();

            string[] pages = "http://192.168.0.122/sounditout/RecordAttempt.php".Split('/');
            int page = pages.Length - 1;

            switch (www.result) {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + www.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + www.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + www.downloadHandler.text);

                    string[] soundexCodes = www.downloadHandler.text.Split(' ');
                    CalculatePhoneticDistance(soundexCodes);
                    StartCoroutine(UpdatePhoneticDistance(attemptId));
                    break;
            }
        }
    }

    protected IEnumerator UpdatePhoneticDistance(string attemptId) {
        WWWForm form = new WWWForm();
        // Send the necessary attributes to the API
        form.AddField("id", attemptId);
        form.AddField("phoneticDistance", phoneticDistance);

        // Call the Web API that updates the skipped value to TRUE in the DB for the given id
        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.122/sounditout/UpdatePhoneticDistance.php", form)) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log(www.error);
            else
                Debug.Log("Phonetic distance updated successfully!");
        }
    }

    // Method to calculate the phonetic distance using two soundex codes
    private void CalculatePhoneticDistance(string[] soundexCodes) {
        phoneticDistance = 0;
        for (int i=0; i<4; i++) {
            if (soundexCodes[0][i]!=soundexCodes[1][i])
                phoneticDistance++;
        }
    }
}
