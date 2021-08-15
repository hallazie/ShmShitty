using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridInputManager : MonoBehaviour
{

    [SerializeField]
    Camera camera;

    public Action<Vector3> OnMouseHover;
    public Action<Vector3> OnMouseClick;

    public LayerMask gridMask;
    public LayerMask buidlingBlockMask;

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
        if (!CheckInBuildMode())
        {
            var position = RaycastAtMask(gridMask);
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
        return false;
    }

    private void CheckMouseClickEvent()
    {
        if (CheckInBuildMode() && Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            var position = RaycastAtMask(buidlingBlockMask);
            if (position != null)
            {
                OnMouseClick?.Invoke(position.Value);
            }
        }
    }
}
