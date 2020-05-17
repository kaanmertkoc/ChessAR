using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    
    [SerializeField] GameObject planePrefab;
    [SerializeField] GameObject blackQueenPrefab;
    [SerializeField] GameObject whiteQueenPrefab;

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

        // First condition is for checking, if the user pressed the left click. Second and third condition are for checking if user pressed an object with these coordinates.
        // Last one is for not be able to press anyting else if the user selected this object. 
        if (Input.GetMouseButtonDown(0) && boardManager.selectionX == (int)transform.position.x && boardManager.selectionY == (int)transform.position.z && chess.isClickable)
        {
            
            Move[] moveArray = CheckPossibleMoves();
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
                else if(chess.turnOfColour.Equals("White"))
                {
                    chess.turnOfColour = "Black";
                }

                destroyObjects("Plane");
                isClicked = false;
                chess.isClickable = true;

                if (gameObject.tag.Equals("Black") && pos.z == 7.5f)
                {
                    promotion();
                }
                else if (gameObject.tag.Equals("White") && pos.z == 0.5f)
                {
                    promotion();
                }
            }
        }
    }
    public bool CheckIfCanItMove(Move[] array)
    {
        int counter = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if (!array[i].GetIsAllowed())
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

    public Move[] CheckPossibleMoves()
    {
       // A pawn can go four different ways, if it's in default position.
       // Two up, one up and two eating position. First if is for white pawn case.
        if(gameObject.CompareTag("Black") && transform.position.z == 1.5f)
        {
           
            Move[] moveArray = new Move[5];

            moveArray[0] = new Move(transform.position, false);

            Vector3 pos = new Vector3(transform.position.x, 0.2f, transform.position.z);

            bool canMove = isMoveAllowed(pos, new Vector3(pos.x, pos.y, pos.z + 1f));
            moveArray[1] = new Move(new Vector3(pos.x, pos.y, pos.z + 1f), canMove);
            
            // If it can't move one up, it is safe to say it can't move two up.
            // So there is no need to check it again.
            if (!canMove)
                moveArray[2] = new Move(new Vector3(pos.x, pos.y, pos.z + 2f), false);
            else
            {
                canMove = isMoveAllowed(pos, new Vector3(pos.x, pos.y, pos.z + 2f));
                moveArray[2] = new Move(new Vector3(pos.x, pos.y, pos.z + 2f), canMove);
            }
            
            canMove = isMoveAllowed(pos, new Vector3(pos.x + 1f, pos.y, pos.z + 1f));
            moveArray[3] = new Move(new Vector3(pos.x + 1f, pos.y, pos.z + 1f), canMove);

            canMove = isMoveAllowed(pos, new Vector3(pos.x - 1f, pos.y, pos.z + 1f));
            moveArray[4] = new Move(new Vector3(pos.x - 1f, pos.y, pos.z + 1f), canMove);

            return moveArray;
        }

        else if(gameObject.CompareTag("White") && transform.position.z == 6.5f)
        {
            Move[] moveArray = new Move[5];

            moveArray[0] = new Move(transform.position, false);

            Vector3 pos = new Vector3(transform.position.x, 0.2f, transform.position.z);

            bool canMove = isMoveAllowed(pos, new Vector3(pos.x, pos.y, pos.z - 1f));
            moveArray[1] = new Move(new Vector3(pos.x, pos.y, pos.z - 1f), canMove);

            if (!canMove)
                moveArray[2] = new Move(new Vector3(pos.x, pos.y, pos.z - 2f), false);
            else
            {
                canMove = isMoveAllowed(pos, new Vector3(pos.x, pos.y, pos.z - 2f));
                moveArray[2] = new Move(new Vector3(pos.x, pos.y, pos.z - 2f), canMove);
            }

            canMove = isMoveAllowed(pos, new Vector3(pos.x - 1f, pos.y, pos.z - 1f));
            moveArray[3] = new Move(new Vector3(pos.x - 1f, pos.y, pos.z - 1f), canMove);

            canMove = isMoveAllowed(pos, new Vector3(pos.x + 1f, pos.y, pos.z - 1f));
            moveArray[4] = new Move(new Vector3(pos.x + 1f, pos.y, pos.z - 1f), canMove);

            return moveArray;
        }
        else if(gameObject.CompareTag("White"))
        {
            Move[] moveArray = new Move[4];

            moveArray[0] = new Move(new Vector3(transform.position.x, transform.position.y, transform.position.z), false);
            Vector3 pos = new Vector3(transform.position.x, 0.2f, transform.position.z);
            
            bool canMove = isMoveAllowed(pos, new Vector3(pos.x, pos.y, pos.z - 1f));
            moveArray[1] = new Move(new Vector3(pos.x, pos.y, pos.z - 1f), canMove);

            canMove = isMoveAllowed(pos, new Vector3(pos.x - 1f, pos.y, pos.z - 1f));
            moveArray[2] = new Move(new Vector3(pos.x - 1f, pos.y, pos.z - 1f), canMove);

            canMove = isMoveAllowed(pos, new Vector3(pos.x + 1f, pos.y, pos.z - 1f));
            moveArray[3] = new Move(new Vector3(pos.x + 1f, pos.y, pos.z - 1f), canMove);

            return moveArray;

        }
        else
        {
            Move[] moveArray = new Move[4];

            moveArray[0] = new Move(new Vector3(transform.position.x, transform.position.y, transform.position.z), false);
            Vector3 pos = new Vector3(transform.position.x, 0.2f, transform.position.z);

            bool canMove = isMoveAllowed(pos, new Vector3(pos.x, pos.y, pos.z + 1f));
            moveArray[1] = new Move(new Vector3(pos.x, pos.y, pos.z + 1f), canMove);

            canMove = isMoveAllowed(pos, new Vector3(pos.x - 1f, pos.y, pos.z + 1f));
            moveArray[2] = new Move(new Vector3(pos.x - 1f, pos.y, pos.z + 1f), canMove);

            canMove = isMoveAllowed(pos, new Vector3(pos.x + 1f, pos.y, pos.z + 1f));
            moveArray[3] = new Move(new Vector3(pos.x + 1f, pos.y, pos.z + 1f), canMove);

            return moveArray;
        }
    }
    // This method creates plane if coordinates is in move array.
    public void createPlanesForAllowedCoordinates(Move[] array)
    {
        // array.GetLength(0) returns the first dimension size of array. Dumb syntax...
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].GetIsAllowed())
            {
                Vector3 pos = array[i].GetPosition();
                createANewPlane(new Vector3(pos.x, 0.2f, pos.z));
            }
        }
    }
    // Promotes pawnes to queen if condition is satisfied.
    public void promotion()
    {
        if(gameObject.tag.Equals("Black"))
        {
            blackQueenPrefab.transform.position = transform.position;
            Destroy(gameObject);
            Instantiate<GameObject>(blackQueenPrefab);
        }
        else if(gameObject.tag.Equals("White"))
        {
            whiteQueenPrefab.transform.position = transform.position;
            Destroy(gameObject);
            Instantiate<GameObject>(whiteQueenPrefab);
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
        // If white pawn is in default position. This lines will execute.
        if(gameObject.CompareTag("White") && nowPos.z == 6.5f)
        {
            string tagOfObject = DetermineTheTag(wantedPos);
            // If tagOfObject equals untagged, it is white or black. Since pawn's can not move down when there is a object, it will return false.
            if (nowPos.x == wantedPos.x && nowPos.z - 1f == wantedPos.z && tagOfObject.Equals(" "))
            {
                return true;
            }
            else if (nowPos.x == wantedPos.x && nowPos.z - 2f == wantedPos.z && tagOfObject.Equals(" "))
            {
                return true;
            }
            // Second condtion is for elimination of opposite color. Since i know this object is white i can compare it with black.
            else if (nowPos.x - 1f == wantedPos.x && nowPos.z - 1f == wantedPos.z && tagOfObject.Equals("Black"))
            {
                return true;
            }
            else if (nowPos.x + 1f == wantedPos.x && nowPos.z - 1f == wantedPos.z && tagOfObject.Equals("Black"))
            {
                return true;
            }
            else
                return false;
        }
        else if(gameObject.CompareTag("Black") && nowPos.z == 1.5f)
        {
            string tagOfObject = DetermineTheTag(wantedPos);
            
            if (nowPos.x == wantedPos.x && nowPos.z + 1f == wantedPos.z && tagOfObject.Equals(" "))
            {
                return true;
            }
            else if (nowPos.x == wantedPos.x && nowPos.z + 2f == wantedPos.z && tagOfObject.Equals(" "))
            {
                return true;
            }
            // Second condtion is for elimination of opposite color. Since i know this object is white i can compare it with black.
            else if (nowPos.x - 1f == wantedPos.x && nowPos.z + 1f == wantedPos.z && tagOfObject.Equals("White"))
            {
                return true;
            }
            else if (nowPos.x + 1f == wantedPos.x && nowPos.z + 1f == wantedPos.z && tagOfObject.Equals("White"))
            {
                return true;
            }
            else
                return false;
        }
        else if(gameObject.CompareTag("Black"))
        {
            string tagOfObject = DetermineTheTag(wantedPos);
            if (nowPos.x == wantedPos.x && nowPos.z + 1f == wantedPos.z && tagOfObject.Equals(" "))
            {
                return true;
            }
            // Second condtion is for elimination of opposite color. Since i know this object is white i can compare it with black.
            else if (nowPos.x - 1f == wantedPos.x && nowPos.z + 1f == wantedPos.z && tagOfObject.Equals("White"))
            {
                return true;
            }
            else if (nowPos.x + 1f == wantedPos.x && nowPos.z + 1f == wantedPos.z && tagOfObject.Equals("White"))
            {
                return true;
            }
            else
                return false;
        }
        else
        {
            string tagOfObject = DetermineTheTag(wantedPos);
            // If tagOfObject equals untagged, it is white or black. Since pawn's can not move down when there is a object, it will return false.
            if (nowPos.x == wantedPos.x && nowPos.z - 1f == wantedPos.z && tagOfObject.Equals(" "))
            {
                return true;
            }
            // Second condtion is for elimination of opposite color. Since i know this object is white i can compare it with black.
            else if (nowPos.x - 1f == wantedPos.x && nowPos.z - 1f == wantedPos.z && tagOfObject.Equals("Black"))
            {
                return true;
            }
            else if (nowPos.x + 1f == wantedPos.x && nowPos.z - 1f == wantedPos.z && tagOfObject.Equals("Black"))
            {
                return true;
            }
            else
                return false;
        }
        
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
        planePrefab.transform.position = coordinates;
        Instantiate<GameObject>(planePrefab);
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
