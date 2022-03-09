using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField]
    private GameObject[] characters;
    private int currCharacterIndex;

    // Start is called before the first frame update
    void Start()
    {
        currCharacterIndex = 0;
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
                continue;
            }
            else {
                characters[i].SetActive(true);
            }
        }
    }
}
