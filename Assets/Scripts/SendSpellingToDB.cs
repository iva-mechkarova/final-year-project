using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendSpellingToDB : MonoBehaviour {
    protected string id;
    protected string userId;
    protected string targetWord;
    protected int repeatCount = 0; // Count how many times repeat btn is pressed

    protected IEnumerator RecordAskedWord() {
        WWWForm form = new WWWForm();
        // Send the necessary attributes to the API
        form.AddField("id", id);
        form.AddField("userId", userId);
        form.AddField("targetWord", targetWord);

        // Call the Web API that records the asked word to the MySQL DB
        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.122/sounditout/RecordAskedWord.php", form)) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log(www.error);
            else
                Debug.Log("Asked word registered successfully!");
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
}
