using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObject : MonoBehaviour
{
    [SerializeField]
    GameObject ChessPrefab;

    public GameObject ChessObject
    {
        get { return ChessPrefab; }
        set { ChessPrefab = value; }
    }

    public GameObject spawnedBoard { get; private set; }

    [SerializeField]
    GameObject BoardManager;
    [SerializeField]
    GameObject Chess;
    [SerializeField]
    GameObject EventManager;

    public static event Action onPlacedObject;

    private bool isPlaced = false;

    ARRaycastManager m_RaycastManager;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    //Updates the position of the object in the real world according to the touched place.
    void Update()
    {
        if (isPlaced)
            return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = s_Hits[0].pose;
                    spawnedBoard = Instantiate(ChessPrefab, hitPose.position,hitPose.rotation);
                    isPlaced = true;

                    

                    isPlaced = true;
                    if (onPlacedObject != null)
                    {
                        onPlacedObject();
                    }
                }
            }
        }
    }
}