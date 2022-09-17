using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSystem : MonoBehaviour
{

    public LayerMask canInteract;
    public LayerMask canMove;

    private Camera _mainCamera;


    private void Start()
    {
        _mainCamera = Camera.main;
    }

    public RaycastHit CantInteract()
    {
        return _CheckRayOnLayer(canInteract);
    }
    
    public RaycastHit CanDrag()
    {
        return _CheckRayOnLayer(canMove);
    }

    private RaycastHit _CheckRayOnLayer(LayerMask layers)
    {
        RaycastHit hit;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, layers))
        {
            return hit;
        }
        return hit;
    }
}
