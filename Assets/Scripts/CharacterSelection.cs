using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField]
    private GameObject[] characters; // Array of character images

    [SerializeField]
    private GameObject[] costs; // Array of costs of each character

    private int currCharacterIndex;
    private int playerScore;

    public Button unlockButton;
    public Button selectButton;
    public GameObject selectedText;

    // Start is called before the first frame update
    void Start()
    {
        currCharacterIndex = 0; // Show first character
        // Get accumulated score or if player hasn't played yet set score to 0
        playerScore = PlayerPrefs.HasKey("totalScore") ? PlayerPrefs.GetInt("totalScore") : 0;
        CheckStatusOfCharacter(0); // Check if character is unlocked, locked or selected
    }

    // Show character to the right of current one
    public void RightBtnPressed() {
        // If curr indx = num characters-1 then we have reached last character so show character at indx 0
        currCharacterIndex = currCharacterIndex < characters.Length-1 ? currCharacterIndex+1 : 0;
        NextCharacter();
    }

    // Show character to the left of current one
    public void LeftBtnPressed() {
        // If curr indx = 0 then we have reached first character so show last character
        currCharacterIndex = currCharacterIndex > 0 ? currCharacterIndex-1 : characters.Length-1;
        NextCharacter();
    }

    private void NextCharacter() {   
        for (int i=0; i<characters.Length; i++) {
            // Deactivate all characters that aren't selected
            if (i != currCharacterIndex) {
                characters[i].SetActive(false);
                costs[i].SetActive(false);
            }
            else {
                CheckStatusOfCharacter(i);
            }
        }
    }

    // Method to switch character i.e. character that is used in the game
    public void SelectButtonPressed() {
        PlayerPrefs.SetInt("selectedCharacter", currCharacterIndex);
        CheckStatusOfCharacter(currCharacterIndex);
    }

    // Method to unlock a character
    public void UnlockButtonPressed() {
        // Store that the character has been unlocked
        PlayerPrefs.SetInt("fish-"+currCharacterIndex.ToString(), 1);
        // Subtract cost from total score and save new total
        playerScore -= GetCurrCharacterCost();
        PlayerPrefs.SetInt("totalScore", playerScore);
        CheckStatusOfCharacter(currCharacterIndex);
    }

    // Method to update what is shown for the current character
    private void CheckStatusOfCharacter(int i) {
        selectedText.SetActive(false); // Deactivate selected text
        characters[i].SetActive(true); // Show current character
        selectButton.gameObject.SetActive(false); // Deactivate select btn
        // Check if character is unlocked by checking if it is saved in PlayerPrefs
        int isUnlocked = PlayerPrefs.HasKey("fish-"+i.ToString()) ? PlayerPrefs.GetInt("fish-"+i.ToString()) : 0;

        // If curr player is unlocked
        if (isUnlocked == 1) {
            costs[i].SetActive(false); // Don't display cost as unlocked already
            unlockButton.gameObject.SetActive(false); // Diactivate unlock btn

            // Check if this is the currently selected character
            if (PlayerPrefs.GetInt("selectedCharacter") == i)
                selectedText.SetActive(true);
            else
                selectButton.gameObject.SetActive(true);
        }
        else {
            CanAffordCharacter();
            unlockButton.gameObject.SetActive(true);
            costs[i].SetActive(true); // Display cost of character as not unlocked
        }
    }

    // Method to check if player has enough accumulated stars to unlock a character
    private void CanAffordCharacter() {
        int cost = GetCurrCharacterCost();
        if (playerScore >= cost) {
            unlockButton.interactable = true;
        }
        else {
            unlockButton.interactable = false;
        }
    }

    // Method to get the cost from the curr character's cost text
    private int GetCurrCharacterCost() {
        string costText = costs[currCharacterIndex].transform.GetChild(0).gameObject.GetComponent<Text>().text;
        return int.Parse(costText);
    }
}
