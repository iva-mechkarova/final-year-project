using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

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
        string id = Guid.NewGuid().ToString(); // Generate a new unique identifier
        Debug.Log("Guid: " + id); 
        // Store the player's age group and id locally for fast access
        PlayerPrefs.SetInt("ageGroup", age);
        PlayerPrefs.SetString("id", id);

        // Store the player's age group and id in the DB
        StartCoroutine(RegisterUser(id, age));

        welcomePanel.SetActive(false);
    }

    public void ConsentToggled() {
        foreach (Button button in ageButtons) {
            button.interactable = !button.interactable;
        }
    }

    private IEnumerator RegisterUser(string id, int age) {
        WWWForm form = new WWWForm();
        // Send the generated unique id and ageGroup to the DB
        form.AddField("id", id);
        form.AddField("ageGroup", age);

        // Call the Web API that registers the user to the MySQL DB
        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.122/sounditout/RegisterUser.php", form)) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log(www.error);
            else
                Debug.Log("Form upload complete!");
        }
    }
}
