using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public SpriteRenderer[] instruments;
    public AudioSource[] buttonSounds;

    private int instrumentSelect;

    public float stayLit;
    private float stayLitCounter;

    public float waitBetweenLights;
    private float waitBetweenCounter;

    private bool shouldBeLit;
    private bool shouldBeDark;

    public List<int> activeSequence;
    private int positionInSequence;

    private bool gameActive;
    private int inputInSequence;

    public AudioSource correct;
    public AudioSource incorrect;

    public TextMeshProUGUI scoreText;

    void Start()
    {
        //scoreText = "Score: 0 - High Score: ";
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldBeLit)
        {
            stayLitCounter -= Time.deltaTime;
            if (stayLitCounter < 0)
            {
                instruments[activeSequence[positionInSequence]].color = new Color(instruments[activeSequence[positionInSequence]].color.r, instruments[activeSequence[positionInSequence]].color.g, instruments[activeSequence[positionInSequence]].color.b, 0.7f);
                buttonSounds[activeSequence[positionInSequence]].Stop();
                shouldBeLit = false;

                shouldBeDark = true;
                waitBetweenCounter = waitBetweenLights;

                positionInSequence++;
            }
        }
        if (shouldBeDark)
        {
            waitBetweenCounter -= Time.deltaTime;
            
            if(positionInSequence >= activeSequence.Count)
            {
                shouldBeDark = false;
                gameActive = true;
            }
            else
            {
                if (waitBetweenCounter < 0)
                {

                    instruments[activeSequence[positionInSequence]].color = new Color(instruments[activeSequence[positionInSequence]].color.r, instruments[activeSequence[positionInSequence]].color.g, instruments[activeSequence[positionInSequence]].color.b, 1f);
                    buttonSounds[activeSequence[positionInSequence]].Play();

                    stayLitCounter = stayLit;
                    shouldBeLit = true;
                    shouldBeDark = false;
                }
            }
        }
    }

    public void StartGame()
    {
        activeSequence.Clear();

        positionInSequence = 0;
        inputInSequence = 0;

        instrumentSelect = Random.Range(0, instruments.Length);

        activeSequence.Add(instrumentSelect);

        instruments[activeSequence[positionInSequence]].color = new Color(instruments[activeSequence[positionInSequence]].color.r, instruments[activeSequence[positionInSequence]].color.g, instruments[activeSequence[positionInSequence]].color.b, 1f);
        buttonSounds[activeSequence[positionInSequence]].Play();
        
        stayLitCounter = stayLit;
        shouldBeLit = true;
    }

    public void ColorPressed(int whichButton)
    {
        if (gameActive)
        {

            if (activeSequence[inputInSequence] == whichButton)
            {
                Debug.Log("Correct");
                correct.Play();

                inputInSequence++;

                if(inputInSequence >= activeSequence.Count)
                {
                    positionInSequence = 0;
                    inputInSequence = 0;

                    instrumentSelect = Random.Range(0, instruments.Length);

                    activeSequence.Add(instrumentSelect);

                    instruments[activeSequence[positionInSequence]].color = new Color(instruments[activeSequence[positionInSequence]].color.r, instruments[activeSequence[positionInSequence]].color.g, instruments[activeSequence[positionInSequence]].color.b, 1f);
                    buttonSounds[activeSequence[positionInSequence]].Play();

                    stayLitCounter = stayLit;
                    shouldBeLit = true;

                    gameActive = false;
                }
            }
            else
            {
                Debug.Log("Wrong");
                incorrect.Play();
                gameActive = false;
            }
        }
    }
}
