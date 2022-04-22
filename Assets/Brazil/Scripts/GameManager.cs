using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SpriteRenderer[] instruments;

    private int instrumentSelect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        instrumentSelect = Random.Range(0, instruments.Length);

        
    }
}
