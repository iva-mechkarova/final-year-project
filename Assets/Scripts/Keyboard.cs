using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    public Text typedWordText;
    public Text placeholderText;

    public void onLetterPressed(string letter) {
        typedWordText.text += letter;
        placeholderText.gameObject.SetActive(false);
    }

    public void onBackspacePressed() {
        if (typedWordText.text.Length > 0) {
            typedWordText.text = typedWordText.text.Substring(0, typedWordText.text.Length - 1);

            if (typedWordText.text.Length == 0)
                placeholderText.gameObject.SetActive(true);
        }
    }
}
