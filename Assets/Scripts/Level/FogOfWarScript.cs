using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarScript : MonoBehaviour
{   
    private Animator _animator;
    private bool _fogIsActive = true;

    private void Start(){
        _animator = gameObject.GetComponent<Animator>();
    }
    public void OnTriggerEnter(Collider other){

        if(other.gameObject.tag == "Player"){

            if(!_fogIsActive) return;
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                transform.parent.GetChild(i).gameObject.GetComponent<FogOfWarScript>().StartFadeOut();
                //transform.parent.GetChild(i).gameObject.SetActive(false);
            }
            _fogIsActive = false;
        }
        
    }

    public void StartFadeOut(){
        _animator.SetBool("FadeOut", true);
    }

    public void DisActivateFog(){
    
        if(!gameObject.activeSelf) return;
        if(!_fogIsActive) return;
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).gameObject.GetComponent<FogOfWarScript>().StartFadeOut();
            //transform.parent.GetChild(i).gameObject.SetActive(false);
        }
        _fogIsActive = false;
    }

    public void DestroyGameobject(){
        Destroy(gameObject);
    }
}
