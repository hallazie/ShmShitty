using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GraphInputManager graphInputManager;
    public GraphVisualizeOperator graphVisualizeOperator;
    public SimplePlacingHandler simplePlacingHandler;

    public void Start()
    {
        InitHoverQuadIndicateHandler();
        InitPlacementHandler();
    }


    private void InitHoverQuadIndicateHandler()
    {
        graphInputManager.OnMouseHover += graphVisualizeOperator.HighlightHoverVertex;
    }

    private void InitPlacementHandler()
    {
        graphInputManager.OnMouseClick += simplePlacingHandler.SimplePlaceHouse;
    }

}
