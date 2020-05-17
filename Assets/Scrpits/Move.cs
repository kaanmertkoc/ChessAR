using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    private Vector3 position;
    private bool isAllowed;
    public Move(Vector3 position, bool isAllowed)
    {
        this.position = position;
        this.isAllowed = isAllowed;
    }
    //Sets the initial position of the game object to the wanted position.
    public void SetPosition(Vector3 position)
    {
        this.position = position;
    }
    //Gets the initial position of the game object
    public Vector3 GetPosition()
    {
        return position;
    }
    //Sets the condition to false or true according to the availability of given position.
    public void SetIsAllowed(bool isAllowed)
    {
        this.isAllowed = isAllowed;
    }
    //Gets the condition of the availability of the given position.
    public bool GetIsAllowed()
    {
        return isAllowed;
    }
    //Displays the avaliability of the position.
    public override string ToString()
    {
        return position + " " + isAllowed;
    }

}
