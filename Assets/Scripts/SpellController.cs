using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class SpellController : MonoBehaviour
{
    private const string LANG_CODE = "en-US";
    private string targetWord = "Television";
    public Text typedWord;
    public Text incorrectText;

    // Start is called before the first frame update
    void Start() {
        SetupTTS(LANG_CODE);
        GetRandomTargetWord();
        StartSpeaking();
    }

    public void StartSpeaking() {
        TextToSpeech.instance.StartSpeak(targetWord);
        Debug.Log("Speaking: " + targetWord);
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

    private void SetupTTS(string code) {
        TextToSpeech.instance.Setting(code, 1, 1);
    }

    private void GetRandomTargetWord() {
        TextAsset words = Resources.Load<TextAsset>("targetWords");
        int lineNumber = Random.Range(1, 11);
        using (StreamReader sr = new StreamReader(new MemoryStream(words.bytes))) {
            for (int i = 1; i < lineNumber; i++)
                sr.ReadLine();
            targetWord = sr.ReadLine();
        }
    }  
}
