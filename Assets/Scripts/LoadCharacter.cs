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
        Instantiate(characterPrefabs[0], transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
