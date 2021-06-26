using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraMovement cameraMovement;
    public InputManager inputManager;
    public RoadManager roadManager;
    public UIController uIController;

    private void Start()
    {
        uIController.OnRoadPlacement += RoadPlacementHandler;
        uIController.OnHousePlacement += HoushPlacementHandler;
        uIController.OnSpecialPlacement += SpacialPlacementHandler;
    }

    private void SpacialPlacementHandler()
    {
        ClearInputActions();
    }

    private void HoushPlacementHandler()
    {
        ClearInputActions();
    }

    private void RoadPlacementHandler()
    {
        ClearInputActions();
        inputManager.OnMouseClick += roadManager.PlaceRoad;
        inputManager.OnMouseHold += roadManager.PlaceRoad;
        inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
    }

    private void ClearInputActions()
    {
        inputManager.OnMouseClick = null;
        inputManager.OnMouseHold = null;
        inputManager.OnMouseUp = null; 
    }

    private void HandleMouseClick(Vector3Int position)
    {
        Debug.Log(position);
    }

    private void Update()
    {
        cameraMovement.MoveCamera(new Vector3(inputManager.CameraMovementVector.x, 0, inputManager.CameraMovementVector.y)); 
    }

}
