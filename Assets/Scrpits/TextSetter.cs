using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSetter : MonoBehaviour
{
    Chess chess;
    [SerializeField] UnityEngine.UI.Text whoWonText;
    //Sets the text which displays who won the game.
    void Start()
    {
        chess = GameObject.Find("Chess").GetComponent<Chess>();
        whoWonText.text = chess.GetWhoWon() + " Won!";
    }

   
}
