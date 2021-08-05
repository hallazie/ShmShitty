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

    void Update()
    {
        
    }

    private void CheckMouseHoverEvent()
    {
        if (!CheckInBuildMode())
        {
            var position = RaycastGround();
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
        throw new NotImplementedException();
    }

    private void CheckMouseClickEvent()
    {

    }
}
