using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsentToggle : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;

    public void ConsentToggled() {
        button1.interactable = !button1.interactable;
        button2.interactable = !button2.interactable;
        button3.interactable = !button3.interactable;
        button4.interactable = !button4.interactable;
        button5.interactable = !button5.interactable;
    }
}
