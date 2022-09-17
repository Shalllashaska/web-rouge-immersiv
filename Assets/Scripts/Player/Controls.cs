using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(InteractSystem))]
[RequireComponent(typeof(InteractObjects))]
[RequireComponent(typeof(CombatSystem))]
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
    private InteractObjects _interactObjects;


    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _combatSystem = gameObject.GetComponent<CombatSystem>();
        _mainCamera = Camera.main;
        _interactSystem = gameObject.GetComponent<InteractSystem>();
        _interactObjects = gameObject.GetComponent<InteractObjects>();
        
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
            InteractObjects(true);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            InteractObjects(false);
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
        RaycastHit hitInteract = _interactSystem.CantInteract();
        if(hitInteract.collider != null){
             if(Vector3.Distance(transform.position, hitInteract.collider.transform.position) > interactDistance) return;
             //Interact Door
             if(hitInteract.collider.tag == "Button"){
                 hitInteract.collider.transform.GetComponentInParent<ButtonInteract>().Interact();
             }
        }
    }
    
    private void InteractObjects(bool takeObject){
        if (!takeObject)
        {
            _interactObjects.DropObject(true);
            return;
        }
        RaycastHit hitDarg = _interactSystem.CanDrag();
        if(hitDarg.collider != null){
            if(Vector3.Distance(transform.position, hitDarg.collider.transform.position) > interactDistance) return;
            _interactObjects.TakeObject(hitDarg.collider.gameObject, transform.localScale.y / 2);
        }
    }
}
