using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    [SerializeField]
    private GameObject[] characterPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        int selectedCharacter = PlayerPrefs.HasKey("selectedCharacter") ? PlayerPrefs.GetInt("selectedCharacter") : 0;
        Instantiate(characterPrefabs[selectedCharacter], transform);
    }
}
