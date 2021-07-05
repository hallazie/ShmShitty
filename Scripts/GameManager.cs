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
    public StructureManager structureManager;
    public UIController uIController;

    private void Start()
    {
        uIController.OnRoadPlacement += RoadPlacementHandler;
        uIController.OnHousePlacement += HoushPlacementHandler;
        uIController.OnSpecialPlacement += SpacialPlacementHandler;
        uIController.OnBigPlacement += BigPlacementHandler;
    }

    private void SpacialPlacementHandler()
    {
        ClearInputActions();
        inputManager.OnMouseClick += structureManager.PlaceSpecial;
    }

    private void HoushPlacementHandler()
    {
        ClearInputActions();
        inputManager.OnMouseClick += structureManager.PlaceHouse;
    }

    private void RoadPlacementHandler()
    {
        ClearInputActions();
        inputManager.OnMouseClick += roadManager.PlaceRoad;
        inputManager.OnMouseHold += roadManager.PlaceRoad;
        inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
    }

    private void BigPlacementHandler()
    {
        ClearInputActions();
        inputManager.OnMouseClick += structureManager.placeBig;
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
