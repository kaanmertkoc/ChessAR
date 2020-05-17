using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //Reloads the chess game scene
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }
    //Exits the game.
    public void Exit()
    {
        Application.Quit();
    }
    //Loads the chess game scene.
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

}
