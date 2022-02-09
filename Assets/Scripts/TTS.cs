using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EasyTTSUtil.Initialize (EasyTTSUtil.UnitedStates);
    }

    public void testTTS() {
        EasyTTSUtil.SpeechAdd("test speech");
        EasyTTSUtil.SpeechFlush("test speech");
    }
}
