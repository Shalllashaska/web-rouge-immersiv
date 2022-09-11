using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSystem : MonoBehaviour
{

    public LayerMask canInteract;

    private Camera _mainCamera;


    private void Start()
    {
        _mainCamera = Camera.main;
    }

    public RaycastHit CantInteract()
    {
        RaycastHit hit;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, canInteract))
        {
            return hit;
        }
        return hit;
    }
}
