using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    private bool _IsOpened = false;
    private Animator _animator;
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    public void InteractDoor(){
        _IsOpened = !_IsOpened;
        _animator.SetBool("IsOpened", _IsOpened);
    }
}
