using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
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
        if(chess.turnOfColour == gameObject.tag)
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
            Move[] moveArray = CheckPossibleMoves();
            // If there is no place piece can move, it doesn't need to call below methods and set below booleans.
            if(CheckIfCanItMove(moveArray))
            {
                chess.PlayClick();   
                createPlanesForAllowedCoordinates(moveArray);
                isClicked = true;
                chess.isClickable = false;
            }
        }
        if (isClicked == true && Input.GetMouseButtonDown(0))
        {
            Move[] moveArray = CheckPossibleMoves();
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
    public void MoveIfAllowed(Move[] array, Vector3 pos)
    {
        for (int i = 0; i < array.Length; i++)
        {
                Debug.Log(array[i].ToString());
                if (array[i].GetPosition() == pos && array[i].GetIsAllowed() == true)
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
    // This method checks if selected piece has a possible move. If it doesn't have a possible move, it doesn't set isClicked and isClickable booleans so that player can select another piece.
    public bool CheckIfCanItMove(Move[] array) 
    {
        int counter = 0;
        for(int i = 0; i < array.Length; i++)
        {
            if(!array[i].GetIsAllowed())
            {
                counter++;
            }
        }
        if (counter == array.Length)
            return false;
        else
            return true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (chess.tag == "Black" && other.gameObject.CompareTag("White"))
        {
            if(other.gameObject.name.Equals("king white"))
            {
                chess.SetWhoWon("Black");
            }
            Destroy(other.gameObject);
        }
        else if (chess.tag == "White" && other.gameObject.CompareTag("Black"))
        {
            if(other.gameObject.name.Equals("king black"))
            {
                chess.SetWhoWon("White");
            }
            Destroy(other.gameObject);
        }
    }

    public Move[] CheckPossibleMoves()
    {
        // This is c# syntax for declaring a 2d array length of 4 and 8. I know it is dumb.
        Move[] moveArray = new Move[9];

        // Setting the objects current position to false.
        moveArray[0] = new Move(transform.position, false);

        Vector3 pos = new Vector3(transform.position.x, 0.2f, transform.position.z);

        // two left one up

        bool canMove = isMoveAllowed(pos, new Vector3(pos.x - 2f, pos.y, pos.z + 1f));
        moveArray[1] = new Move(new Vector3(pos.x - 2f, pos.y, pos.z + 1f), canMove);

        // two up one left

        canMove = isMoveAllowed(pos, new Vector3(pos.x - 1f, pos.y, pos.z + 2f));
        moveArray[2] = new Move(new Vector3(pos.x - 1f, pos.y, pos.z + 2f), canMove);

        // two right one up

        canMove = isMoveAllowed(pos, new Vector3(pos.x + 2f, pos.y, pos.z + 1f));
        moveArray[3] = new Move(new Vector3(pos.x + 2f, pos.y, pos.z + 1f), canMove);

        // two up one right

        canMove = isMoveAllowed(pos, new Vector3(pos.x + 1f, pos.y, pos.z + 2f));
        moveArray[4] = new Move(new Vector3(pos.x + 1f, pos.y, pos.z + 2f), canMove);

        // two down one left

        canMove = isMoveAllowed(pos, new Vector3(pos.x - 1f, pos.y, pos.z - 2f));
        moveArray[5] = new Move(new Vector3(pos.x - 1f, pos.y, pos.z - 2f), canMove);

        // two down one right

        canMove = isMoveAllowed(pos, new Vector3(pos.x + 1f, pos.y, pos.z - 2f));
        moveArray[6] = new Move(new Vector3(pos.x + 1f, pos.y, pos.z - 2f), canMove);

        // two down one left

        canMove = isMoveAllowed(pos, new Vector3(pos.x - 2f, pos.y, pos.z - 1f));
        moveArray[7] = new Move(new Vector3(pos.x - 2f, pos.y, pos.z - 1f), canMove);

        // two down one right

        canMove = isMoveAllowed(pos, new Vector3(pos.x + 2f, pos.y, pos.z - 1f));
        moveArray[8] = new Move(new Vector3(pos.x + 2f, pos.y, pos.z - 1f), canMove);

        return moveArray;
       
    }
    // This method creates plane if coordinates is in move array.
    public void createPlanesForAllowedCoordinates(Move[] array)
    {
        // array.GetLength(0) returns the first dimension size of array. Dumb syntax...
        for (int i = 0; i < array.Length; i++)
        {
            if(array[i].GetIsAllowed())
            {
                Vector3 pos = array[i].GetPosition();
                createANewPlane(new Vector3(pos.x, 0.2f, pos.z));
            }
        }
    }
    // Return is a boolean array size of two. Because we can't change variables inside method. I tried to change higlightOnce inside the function
    // Even though it is set to false, after executing method highlight once stays the same no matter what. It is because booleans are primitive type and arrays are not.
    // First index of this array is should user can move to this place. Second one is for highlithing once for going to coordinates for elimination and setting it false for not going beyond.
    public bool isMoveAllowed(Vector3 nowPos, Vector3 wantedPos)
    {
        // three things i want to check. First one is if this object can move to this position. Second one is if this position is in the map's boundary.
        // Third one is, is this position have a object that is the same colour of the object that wants to move or not.

        // If either this condition is true, returns false because it doesn't need to check anything else if wanted position outside the map.
        if (wantedPos.x < 0 || wantedPos.z > 8 || wantedPos.x > 8 || wantedPos.z < 0)
        {
            return false;
        }

        string tagOfObject = DetermineTheTag(wantedPos);

        if (nowPos.x - 2f == wantedPos.x && nowPos.z + 1f == wantedPos.z && tagOfObject != gameObject.tag)
        {
            return true;
        }
        else if (nowPos.x - 1f == wantedPos.x && nowPos.z + 2f == wantedPos.z && tagOfObject != gameObject.tag)
        {
            return true;
        }
        else if (nowPos.x + 2f == wantedPos.x && nowPos.z + 1f == wantedPos.z && tagOfObject != gameObject.tag)
        {
            return true;
        }
        else if (nowPos.x + 1f == wantedPos.x && nowPos.z + 2f == wantedPos.z && tagOfObject != gameObject.tag)
        {
            return true;
        }
        else if (nowPos.x - 1f == wantedPos.x && nowPos.z - 2f == wantedPos.z && tagOfObject != gameObject.tag)
        {
            return true;
        }
        else if (nowPos.x + 1f == wantedPos.x && nowPos.z - 2f == wantedPos.z && tagOfObject != gameObject.tag)
        {
            return true;
        }
        else if (nowPos.x - 2f == wantedPos.x && wantedPos.z - 1f == wantedPos.z && tagOfObject != gameObject.tag)
        {
            return true;
        }
        else if (nowPos.x + 2f == wantedPos.x && wantedPos.z - 1f == wantedPos.z && tagOfObject != gameObject.tag)
        {
            return true;
        }
        else
            return false;
        // These if statements returns true if these object can move this position and their tag is not same.
        
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
