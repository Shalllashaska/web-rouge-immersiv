using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteract : MonoBehaviour
{

    public DoorScript _doorScript;
    public FogOfWarScript _fog1;
    public FogOfWarScript _fog2;

    // Start is called before the first frame update
    public void Interact() {

        if(_doorScript){
            _doorScript.InteractDoor();
        }

        if(_fog1){
            _fog1.DisActivateFog();
        }

        if(_fog2){
            _fog2.DisActivateFog();
        }
        
    }
}
