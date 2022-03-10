using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField]
    private GameObject[] characters;

    [SerializeField]
    private GameObject[] costs;

    private int currCharacterIndex;
    private int playerScore;

    public Button unlockButton;
    public Button selectButton;
    public GameObject selectedText;

    // Start is called before the first frame update
    void Start()
    {
        currCharacterIndex = 0;
        playerScore = PlayerPrefs.HasKey("totalScore") ? PlayerPrefs.GetInt("totalScore") : 0;
        CheckStatusOfCharacter(0);
    }

    public void RightBtnPressed() {
        currCharacterIndex = currCharacterIndex < characters.Length-1 ? currCharacterIndex+1 : 0;
        NextCharacter();
    }

    public void LeftBtnPressed() {
        currCharacterIndex = currCharacterIndex > 0 ? currCharacterIndex-1 : characters.Length-1;
        NextCharacter();
    }

    private void NextCharacter() {   
        for (int i=0; i<characters.Length; i++) {
            if (i != currCharacterIndex) {
                characters[i].SetActive(false);
                costs[i].SetActive(false);
            }
            else {
                CheckStatusOfCharacter(i);
            }
        }
    }

    public void SelectButtonPressed() {
        PlayerPrefs.SetInt("selectedCharacter", currCharacterIndex);
        CheckStatusOfCharacter(currCharacterIndex);
    }

    public void UnlockButtonPressed() {
        PlayerPrefs.SetInt("fish-"+currCharacterIndex.ToString(), 1);
        playerScore -= GetCurrCharacterCost();
        PlayerPrefs.SetInt("totalScore", playerScore);
        CheckStatusOfCharacter(currCharacterIndex);
    }

    private void CheckStatusOfCharacter(int i) {
        selectedText.SetActive(false);
        characters[i].SetActive(true);
        selectButton.gameObject.SetActive(false);
        costs[0].SetActive(false);
        int isUnlocked = PlayerPrefs.HasKey("fish-"+i.ToString()) ? PlayerPrefs.GetInt("fish-"+i.ToString()) : 0;
        if (isUnlocked == 1) {
            costs[i].SetActive(false);
            unlockButton.gameObject.SetActive(false);
            if (PlayerPrefs.GetInt("selectedCharacter") == i) {
                selectedText.SetActive(true);
            }
            else {
                selectButton.gameObject.SetActive(true);
                costs[0].SetActive(true);
            }
        }
        else {
            CanAffordCharacter();
            unlockButton.gameObject.SetActive(true);
            costs[i].SetActive(true);
        }
    }

    private void CanAffordCharacter() {
        int cost = GetCurrCharacterCost();
        if (playerScore >= cost) {
            unlockButton.interactable = true;
        }
        else {
            unlockButton.interactable = false;
        }
    }

    private int GetCurrCharacterCost() {
        string costText = costs[currCharacterIndex].transform.GetChild(0).gameObject.GetComponent<Text>().text;
        return int.Parse(costText);
    }
}
