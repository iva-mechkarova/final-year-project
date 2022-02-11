using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    public Text typedWordText;
    public Text placeholderText;

    // Update is called once per frame
    void Update() {
        if (typedWordText.text.Length == 0)
            placeholderText.gameObject.SetActive(true);
    }

    public void onLetterPressed(string letter) {
        typedWordText.text += letter;
        placeholderText.gameObject.SetActive(false);
    }

    public void onBackspacePressed() {
        if (typedWordText.text.Length > 0)
            typedWordText.text = typedWordText.text.Substring(0, typedWordText.text.Length - 1);
    }
}
