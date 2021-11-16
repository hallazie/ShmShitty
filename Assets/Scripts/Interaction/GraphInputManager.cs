using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GraphInputManager : MonoBehaviour
{
    /*
     鼠标在基础平面网格上的interaction
     */

    [SerializeField]
    Camera camera;

    public Action<Vector3> OnMouseHover;
    public Action<Vector3> OnMouseClick;

    public LayerMask graphMask;
    public LayerMask buidlingBlockMask;
    public bool buildingMode = false;
    public bool alwaysShowHover = true;

    public void Start()
    {
        
    }

    public void Update()
    {
        CheckMouseHoverEvent();
        CheckMouseClickEvent();
    }

    private void CheckMouseHoverEvent()
    {
        if (!CheckInBuildMode() || alwaysShowHover)
        {
            var position = RaycastAtMask(graphMask);
            if (position != null)
            {
                OnMouseHover?.Invoke(position.Value);
            }
        }
    }

    private Vector3? RaycastAtMask(LayerMask mask)
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            return hit.point;
        }
        return null;
    }

    private bool CheckInBuildMode()
    {
        return buildingMode;
    }

    private void CheckMouseClickEvent()
    {
        // if (CheckInBuildMode() && Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
        if (CheckInBuildMode() && Input.GetMouseButton(0))
        {
            var position = RaycastAtMask(buidlingBlockMask);
            if (position != null)
            {
                OnMouseClick?.Invoke(position.Value);
            }
        }
    }
}
