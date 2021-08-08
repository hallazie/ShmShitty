using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GridInputManager gridInputManager;
    public VisualizeOperator visualizeOperator;

    public void Start()
    {
        gridInputManager.OnMouseHover += visualizeOperator.HighlightHoverVertex;
    }


    private void HoverQuadIndicateHandler()
    {
        gridInputManager.OnMouseHover += visualizeOperator.HighlightHoverVertex;
    }

}
