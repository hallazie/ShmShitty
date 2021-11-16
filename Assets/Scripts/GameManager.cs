using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GridInputManager gridInputManager;
    public VisualizeOperator visualizeOperator;
    public SimplePlacingHandler simplePlacingHandler;

    public void Start()
    {
        InitHoverQuadIndicateHandler();
        InitPlacementHandler();
    }


    private void InitHoverQuadIndicateHandler()
    {
        gridInputManager.OnMouseHover += visualizeOperator.HighlightHoverVertex;
    }

    private void InitPlacementHandler()
    {
        gridInputManager.OnMouseClick += simplePlacingHandler.SimplePlaceHouse;
    }

}
