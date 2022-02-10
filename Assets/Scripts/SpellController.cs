using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpellController : MonoBehaviour
{
    private const string LANG_CODE = "en-US";
    private string targetWord = "Television";
    public Text typedWord;
    public Text incorrectText;

    // Start is called before the first frame update
    void Start() {
        Setup(LANG_CODE);
        // Code to select targetWord will go here
        StartSpeaking();
    }

    public void StartSpeaking() {
        TextToSpeech.instance.StartSpeak(targetWord);
    }

    public void CheckSpelling() {
        if (!targetWord.ToUpper().Equals(typedWord.text.ToUpper())) {
            incorrectText.gameObject.SetActive(true);
        }
        else {
            incorrectText.gameObject.SetActive(false);
            SceneManager.LoadScene("StartScreen");
        }
    }

    private void Setup(string code) {
        TextToSpeech.instance.Setting(code, 1, 1);
    }
}
