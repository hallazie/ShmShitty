using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInputManager : MonoBehaviour
{

    [SerializeField]
    Camera camera;

    public Action<Vector3> OnMouseHover;

    public LayerMask gridMask;

    public void Start()
    {
        
    }

    public void Update()
    {
        CheckMouseHoverEvent();
    }

    private void CheckMouseHoverEvent()
    {
        if (!CheckInBuildMode())
        {
            var position = RaycastGround();
            // Debug.Log("current position: " + (position == null) + " --" + gridMask.value.ToString());
            if (position != null)
            {
                OnMouseHover?.Invoke(position.Value);
            }
        }
    }

    private Vector3? RaycastGround()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridMask))
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

    }
}
