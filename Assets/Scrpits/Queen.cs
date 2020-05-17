using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : MonoBehaviour
{
    
    [SerializeField] GameObject prefab;
    BoardManager boardManager;
    Chess chess;
    private bool isClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        chess = GameObject.Find("Chess").GetComponent<Chess>();
    }

    // Update is called once per frame
    void Update()
    {
        // I know white pieces has the first move on chess, but that is racism so in this chess game black ones play first.
        if (chess.turnOfColour == gameObject.tag)
        {
            isClickedOn();
        }
        
    }
    public void isClickedOn()
    {
        // These two lines are for set boardManager and chess variable with their class. BoardManager is needed for understanding the coordinates which user selected.
        // Chess is for check if user wants to tap multiple objects and prevent it.
        boardManager = GameObject.Find("ChessBoard").GetComponent<BoardManager>();

        //

        // First condition is for checking, if the user pressed the left click. Second and third condition are for checking if user pressed an object with these coordinates.
        // Last one is for not be able to press anyting else if the user selected this object. 
        if (Input.GetMouseButtonDown(0) && boardManager.selectionX == (int)transform.position.x && boardManager.selectionY == (int)transform.position.z && chess.isClickable)
        {
            Move[,] moveArray = CheckPossibleMoves();
            if (CheckIfCanItMove(moveArray))
            {
                chess.PlayClick();
                createPlanesForAllowedCoordinates(moveArray);
                isClicked = true;
                chess.isClickable = false;
            }
        }
        if (isClicked == true && Input.GetMouseButtonDown(0))
        {
            Move[,] moveArray = CheckPossibleMoves();
            MoveIfAllowed(moveArray, new Vector3(boardManager.selectionX + 0.50f, transform.position.y, boardManager.selectionY + 0.50f));
        }
        if (Input.GetMouseButtonDown(1))
        {
            destroyObjects("Plane");
            chess.isClickable = true;
            isClicked = false;
        }

    }
    // This method checks if wanted position is in the move array and is it avaliable.
    public void MoveIfAllowed(Move[,] array, Vector3 pos)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Debug.Log(array[i, j].ToString());

                if (array[i, j].GetPosition() == pos && array[i, j].GetIsAllowed() == true)
                {
                    Debug.Log("YES!");
                    chess.tag = gameObject.tag;
                    transform.position = pos;
                    chess.PlayMoveSound();
                    // Setting the string in chess class to current colour of the current's piece turn.
                    if (chess.turnOfColour.Equals("Black"))
                    {
                        chess.turnOfColour = "White";
                    }
                    else if (chess.turnOfColour.Equals("White"))
                    {
                        chess.turnOfColour = "Black";
                    }
                    destroyObjects("Plane");
                    isClicked = false;
                    chess.isClickable = true;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (chess.tag == "Black" && other.gameObject.CompareTag("White"))
        {
            if (other.gameObject.name.Equals("king white"))
            {
                chess.SetWhoWon("Black");
            }
            Destroy(other.gameObject);
        }
        else if (chess.tag == "White" && other.gameObject.CompareTag("Black"))
        {
            if (other.gameObject.name.Equals("king black"))
            {
                chess.SetWhoWon("White");
            }
            Destroy(other.gameObject);
        }
    }
    public bool CheckIfCanItMove(Move[,] array)
    {
        int counter = 0;
        // I didn't understand the syntax for checking array's first and second dimension's length. Since i know it must be 8 to 8 i use this way.
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (!array[i, j].GetIsAllowed())
                {
                    counter++;
                }
            }
        }
        if (counter == 8 * 8)
            return false;
        else
            return true;
    }

    public Move[,] CheckPossibleMoves()
    {
        // This is c# syntax for declaring a 2d array length of 4 and 8. I know it is dumb.
        Move[,] moveArray = new Move[8, 8];

        // Setting the objects current position to false.
        for(int i = 0; i < 8; i++)
        {
            moveArray[i, 0] = new Move(transform.position, false);
        }

        // First dimension is for possible moves to up, second is for down, third is for left, fourth is for left.
        int index = 1;
        bool canMove = true;
        bool highlightOnce = true;
        while (index < 8)
        {
            Vector3 pos = new Vector3(transform.position.x + (1f * index), transform.position.y, transform.position.z + (1f * index));
            if (canMove)
            {
                bool[] array = isMoveAllowed(transform.position, pos, 1f * index, highlightOnce);
                canMove = array[0];
                highlightOnce = array[1];
                Debug.Log("highlight " + highlightOnce);
                moveArray[0, index] = new Move(pos, canMove);
            }
            else
            {
                moveArray[0, index] = new Move(pos, false);
            }
            index++;
        }
        canMove = true;
        index = 1;
        highlightOnce = true;
        while (index < 8)
        {
            Vector3 pos = new Vector3(transform.position.x - (1f * index), transform.position.y, transform.position.z + (index * 1f));
            if (canMove)
            {
                bool[] array = isMoveAllowed(transform.position, pos, 1f * index, highlightOnce);
                canMove = array[0];
                highlightOnce = array[1];
                //Debug.Log("highlight " + highlightOnce);
                moveArray[1, index] = new Move(pos, canMove);
            }
            else
            {
                moveArray[1, index] = new Move(pos, false);
            }
            index++;
        }

        canMove = true;
        index = 1;
        highlightOnce = true;
        while (index < 8)
        {
            Vector3 pos = new Vector3(transform.position.x + (index * 1f), transform.position.y, transform.position.z - (index * 1f));
            if (canMove)
            {
                bool[] array = isMoveAllowed(transform.position, pos, 1f * index, highlightOnce);
                canMove = array[0];
                highlightOnce = array[1];
                //Debug.Log("highlight " + highlightOnce);
                moveArray[2, index] = new Move(pos, canMove);
            }
            else
            {
                moveArray[2, index] = new Move(pos, false);
            }
            index++;
        }

        canMove = true;
        index = 1;
        highlightOnce = true;
        while (index < 8)
        {
            Vector3 pos = new Vector3(transform.position.x - (index * 1f), transform.position.y, transform.position.z - (index * 1f));
            if (canMove)
            {
                bool[] array = isMoveAllowed(transform.position, pos, 1f * index, highlightOnce);
                canMove = array[0];
                highlightOnce = array[1];
                //Debug.Log("highlight " + highlightOnce);
                moveArray[3, index] = new Move(pos, canMove);
            }
            else
            {
                moveArray[3, index] = new Move(pos, false);
            }
            index++;
        }
        canMove = true;
        index = 1;
        highlightOnce = true;
        while (index < 8)
        {
            Vector3 pos = new Vector3(transform.position.x + (index * 1f), transform.position.y, transform.position.z);
            if (canMove)
            {
                bool[] array = isMoveAllowed(transform.position, pos, 1f * index, highlightOnce);
                canMove = array[0];
                highlightOnce = array[1];
                //Debug.Log("highlight " + highlightOnce);
                moveArray[4, index] = new Move(pos, canMove);
            }
            else
            {
                moveArray[4, index] = new Move(pos, false);
            }
            index++;
        }
        canMove = true;
        index = 1;
        highlightOnce = true;
        while (index < 8)
        {
            Vector3 pos = new Vector3(transform.position.x - (index * 1f), transform.position.y, transform.position.z);
            if (canMove)
            {
                bool[] array = isMoveAllowed(transform.position, pos, 1f * index, highlightOnce);
                canMove = array[0];
                highlightOnce = array[1];
                //Debug.Log("highlight " + highlightOnce);
                moveArray[5, index] = new Move(pos, canMove);
            }
            else
            {
                moveArray[5, index] = new Move(pos, false);
            }
            index++;
        }
        canMove = true;
        index = 1;
        highlightOnce = true;
        while (index < 8)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + (index * 1f));
            if (canMove)
            {
                bool[] array = isMoveAllowed(transform.position, pos, 1f * index, highlightOnce);
                canMove = array[0];
                highlightOnce = array[1];
                //Debug.Log("highlight " + highlightOnce);
                moveArray[6, index] = new Move(pos, canMove);
            }
            else
            {
                moveArray[6, index] = new Move(pos, false);
            }
            index++;
        }

        canMove = true;
        index = 1;
        highlightOnce = true;
        while (index < 8)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z - (index * 1f));
            if (canMove)
            {
                bool[] array = isMoveAllowed(transform.position, pos, 1f * index, highlightOnce);
                canMove = array[0];
                highlightOnce = array[1];
                //Debug.Log("highlight " + highlightOnce);
                moveArray[7, index] = new Move(pos, canMove);
            }
            else
            {
                moveArray[7, index] = new Move(pos, false);
            }
            index++;
        }

        return moveArray;
        //Debug.Log(isAllowed);
    }
    // This method creates plane if coordinates is in move array.
    public void createPlanesForAllowedCoordinates(Move[,] array)
    {
        // array.GetLength(0) returns the first dimension size of array. Dumb syntax...
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (array[i, j].GetIsAllowed())
                {
                    Vector3 pos = array[i, j].GetPosition();
                    createANewPlane(new Vector3(pos.x, 0.2f, pos.z));
                }
            }
        }
    }
    // Return is a boolean array size of two. Because we can't change variables inside method. I tried to change higlightOnce inside the function
    // Even though it is set to false, after executing method highlight once stays the same no matter what. It is because booleans are primitive type and arrays are not.
    // First index of this array is should user can move to this place. Second one is for highlithing once for going to coordinates for elimination and setting it false for not going beyond.
    public bool[] isMoveAllowed(Vector3 nowPos, Vector3 wantedPos, float distance, bool highlightOnce)
    {
        // three things i want to check. First one is if this object can move to this position. Second one is if this position is in the map's boundary.
        // Third one is, is this position have a object that is the same colour of the object that wants to move or not.

        // If either this condition is true, returns false because it doesn't need to check anything else if wanted position outside the map.
        if (wantedPos.x < 0 || wantedPos.z > 8 || wantedPos.x > 8 || wantedPos.z < 0)
        {
            bool[] returnArray = { false, true };
            return returnArray;
        }

        string tagOfObject = DetermineTheTag(wantedPos);

        // These if statements returns true if these object can move this position and their tag is not same.
        if (nowPos.x + distance == wantedPos.x && nowPos.z + distance == wantedPos.z && tagOfObject != gameObject.tag && highlightOnce)
        {
            bool[] returnArray = { true, true };
            if (tagOfObject.Equals("Black") && gameObject.CompareTag("White"))
            {

                returnArray[1] = false;

            }

            else if (tagOfObject.Equals("White") && gameObject.CompareTag("Black"))
            {
                returnArray[1] = false;
            }


            return returnArray;
        }
        else if (nowPos.x - distance == wantedPos.x && nowPos.z + distance == wantedPos.z && tagOfObject != gameObject.tag && highlightOnce)
        {
            bool[] returnArray = { true, true };
            if (tagOfObject.Equals("Black") && gameObject.CompareTag("White"))
            {

                returnArray[1] = false;

            }

            else if (tagOfObject.Equals("White") && gameObject.CompareTag("Black"))
            {
                returnArray[1] = false;
            }


            return returnArray;
        }
        else if (nowPos.z - distance == wantedPos.z && nowPos.x + distance == wantedPos.x && tagOfObject != gameObject.tag && highlightOnce)
        {
            bool[] returnArray = { true, true };
            if (tagOfObject.Equals("Black") && gameObject.CompareTag("White"))
            {

                returnArray[1] = false;

            }

            else if (tagOfObject.Equals("White") && gameObject.CompareTag("Black"))
            {
                returnArray[1] = false;
            }


            return returnArray;
        }
        else if (nowPos.z - distance == wantedPos.z && nowPos.x - distance == wantedPos.x && tagOfObject != gameObject.tag && highlightOnce)
        {
            bool[] returnArray = { true, true };
            if (tagOfObject.Equals("Black") && gameObject.CompareTag("White"))
            {

                returnArray[1] = false;

            }
            else if (tagOfObject.Equals("White") && gameObject.CompareTag("Black"))
            {
                returnArray[1] = false;
            }
            return returnArray;
        }
        else if (nowPos.z  == wantedPos.z && nowPos.x + distance == wantedPos.x && tagOfObject != gameObject.tag && highlightOnce)
        {
            bool[] returnArray = { true, true };
            if (tagOfObject.Equals("Black") && gameObject.CompareTag("White"))
            {

                returnArray[1] = false;

            }
            else if (tagOfObject.Equals("White") && gameObject.CompareTag("Black"))
            {
                returnArray[1] = false;
            }
            return returnArray;
        }
        else if (nowPos.z == wantedPos.z && nowPos.x - distance == wantedPos.x && tagOfObject != gameObject.tag && highlightOnce)
        {
            bool[] returnArray = { true, true };
            if (tagOfObject.Equals("Black") && gameObject.CompareTag("White"))
            {

                returnArray[1] = false;

            }
            else if (tagOfObject.Equals("White") && gameObject.CompareTag("Black"))
            {
                returnArray[1] = false;
            }
            return returnArray;
        }
        else if (nowPos.z + distance == wantedPos.z && nowPos.x  == wantedPos.x && tagOfObject != gameObject.tag && highlightOnce)
        {
            bool[] returnArray = { true, true };
            if (tagOfObject.Equals("Black") && gameObject.CompareTag("White"))
            {

                returnArray[1] = false;

            }
            else if (tagOfObject.Equals("White") && gameObject.CompareTag("Black"))
            {
                returnArray[1] = false;
            }
            return returnArray;
        }
        else if (nowPos.z - distance == wantedPos.z && nowPos.x  == wantedPos.x && tagOfObject != gameObject.tag && highlightOnce)
        {
            bool[] returnArray = { true, true };
            if (tagOfObject.Equals("Black") && gameObject.CompareTag("White"))
            {

                returnArray[1] = false;

            }
            else if (tagOfObject.Equals("White") && gameObject.CompareTag("Black"))
            {
                returnArray[1] = false;
            }
            return returnArray;
        }
        else
        {

            bool[] returnArray = { false, true };
            return returnArray;
        }
    }
    // Determinates the tag by sending ray at 'em. Returns " " if ray hit nothing.
    public string DetermineTheTag(Vector3 wantedPos)
    {

        Vector3 A = wantedPos;
        Vector3 B = Camera.main.gameObject.transform.position - wantedPos;
        Vector3 direction = (B - A).normalized;
        Debug.DrawRay(A, direction * 10f, Color.green, 10f);
        Ray ray = new Ray(A, direction);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.collider.isTrigger && hit.collider.gameObject.name != gameObject.name && !hit.collider.gameObject.CompareTag("Untagged"))
                return hit.collider.tag.ToString();
            else
            {
                return " ";
            }
        }
        else
        {
            return " ";
        }
    }
    // Creates a new plane from input coordinate.
    private void createANewPlane(Vector3 coordinates)
    {
        prefab.transform.position = coordinates;
        Instantiate<GameObject>(prefab);
    }
    // Destroy the objects with given tag.
    public void destroyObjects(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject target in gameObjects)
        {
            GameObject.Destroy(target);
        }
    }
}
