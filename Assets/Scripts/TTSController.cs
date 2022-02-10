using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;

public class TTSController : MonoBehaviour
{
    const string LANG_CODE = "en-US";

    // Start is called before the first frame update
    void Start() {
        Setup(LANG_CODE);
    }

    #region Text to Speech

    public void StartSpeaking(string wordToSpeak) {
        TextToSpeech.instance.StartSpeak(wordToSpeak);
    }

    #endregion

    private void Setup(string code) {
        TextToSpeech.instance.Setting(code, 1, 1);
    }
}
