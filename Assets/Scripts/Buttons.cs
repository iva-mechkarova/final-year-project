using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void PlayButton() {
        // Start the main game when Play Button is pressed
        SceneManager.LoadScene("MainGame");
    }

    public void SpellButton() {
    }

    public void CharactersButton() {
    }
}
