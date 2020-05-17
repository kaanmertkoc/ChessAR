using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Chess : MonoBehaviour
{

    [SerializeField] AudioClip clickSound;
    [SerializeField] AudioClip moveSound;

    [SerializeField] UnityEngine.UI.Text turnText;
    [SerializeField] UnityEngine.UI.Text checkText;
    [SerializeField] UnityEngine.UI.Button castlingButton;

    public string tag;
    public bool isClickable = true;
    public string turnOfColour = "Black";
    public bool canEatKing = false;
    public bool isKingMoved = false;
    public bool isRookMoved = false;

    // I want to pass a variable through to game over scene. In order to do that i need to declare a static variable.
    // So value will be same in every scene.
    public static string whoWon = "";

    // Start is called before the first frame update
    void Start()
    {
        turnText.text = "It is " + turnOfColour + "'s turn.";
        tag = " ";
    }

    // Update is called once per frame
    void Update()
    {
        turnText.text = "It is " + turnOfColour + "'s turn.";
        
    }
    //Displays if there is a check situation.
    public void DisplayCheck()
    {
        if(canEatKing)
        {
            checkText.text = "Check";
            canEatKing = false;
        }
        else
        {
            checkText.text = " ";
        }
    }
    //Plays click sound.
    public void PlayClick()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = clickSound;
        audio.Play();
    }
    //Plays move sound.
    public void PlayMoveSound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = moveSound;
        audio.Play();
    }
    // Because of my variable is static i can not access on another scripts.
    // Sets who won the match.
    public void SetWhoWon(string value)
    {
        whoWon = value;
        SceneManager.LoadScene(2);
    }
    //Gets who won the match.
    public string GetWhoWon()
    {
        return whoWon;
    }
    
}
