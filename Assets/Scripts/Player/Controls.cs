using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Controls : MonoBehaviour
{
    public float speedRotation = 10;
    public float speedMovement = 10f;

    public float multSpeed = 100f;

    public float interactDistance = 3f;

    private Camera _mainCamera;
    private Vector3 _mouseDirection;
    private Vector3 _currentDir;

    private Rigidbody _rb;


    private Vector3 _movement;
    private CombatSystem _combatSystem;
    private InteractSystem _interactSystem;
    private MapScript _mapScript;


    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _combatSystem = gameObject.GetComponent<CombatSystem>();
        _mainCamera = Camera.main;
        _interactSystem = gameObject.GetComponent<InteractSystem>();
        _mapScript = GameObject.Find("MapHolder").GetComponent<MapScript>();
    }


    void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.z = Input.GetAxisRaw("Vertical");
        _movement.y = 0;
        
        RotatePlayer();
        if(Input.GetMouseButton(0)){
            Shooting();
        }
        if(Input.GetKeyDown(KeyCode.E)){
            Interact();
        }
        if(Input.GetKeyDown(KeyCode.Tab)){
            SwitchMap();
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        _rb.velocity += _movement * speedMovement * multSpeed * Time.fixedDeltaTime;
        //_rb.MovePosition(_rb.position + _movement * speedMovement * Time.fixedDeltaTime);
    }

    private void RotatePlayer()
    {
        _mouseDirection = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.transform.position.y)) - transform.position;
        _mouseDirection.Normalize();
        _mouseDirection = new Vector3(_mouseDirection.x, 0, _mouseDirection.z);
        _currentDir = Vector3.Lerp(_currentDir, _mouseDirection, Time.deltaTime * speedRotation);

        transform.forward = _currentDir;
    }

    private void Shooting(){
        _combatSystem.Shoot();
    }

    private void SwitchMap(){
        if(_mapScript){
            _mapScript.SwitchMap();
        }
        else{
            Debug.Log("Add map on scene");
        }
    }

    private void Interact(){
        RaycastHit hit = _interactSystem.CantInteract();
        if(hit.collider != null){
             if(Vector3.Distance(transform.position, hit.collider.transform.position) > interactDistance) return;
             //Interact Door
             if(hit.collider.tag == "Button"){
                 hit.collider.transform.GetComponentInParent<ButtonInteract>().Interact();
             }
        }
    }
}
