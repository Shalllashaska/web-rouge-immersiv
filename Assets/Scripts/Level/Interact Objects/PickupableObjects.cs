using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableObjects : MonoBehaviour
{
    public bool isWeapon;

    void Start()
    {
        
    }

    public void PickUpObject()
    {
        //TODO Apply object to inventory 
        if (isWeapon)
        {
            
        }


        Destroy(gameObject);
    }   
}
