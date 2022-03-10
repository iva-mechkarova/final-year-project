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

    // Start is called before the first frame update
    void Start()
    {
        currCharacterIndex = 0;
        playerScore = PlayerPrefs.HasKey("totalScore") ? PlayerPrefs.GetInt("totalScore") : 0;
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
                continue;
            }
            else {
                characters[i].SetActive(true);
                int isUnlocked = PlayerPrefs.HasKey("fish-"+i.ToString()) ? PlayerPrefs.GetInt("fish-"+i.ToString()) : 0;
                if (isUnlocked == 1) {
                    costs[i].SetActive(false);
                    costs[0].SetActive(true);
                }
                else {
                    CanAffordCharacter();
                    costs[i].SetActive(true);
                }
            }
        }
    }

    private void CanAffordCharacter() {
        string costText = costs[currCharacterIndex].transform.GetChild(0).gameObject.GetComponent<Text>().text;
        Debug.Log(costText);
        int cost = int.Parse(costText);
        if (playerScore >= cost) {
            unlockButton.interactable = true;
        }
        else {
            unlockButton.interactable = false;
        }
    }
}
