using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(InteractSystem))]
[RequireComponent(typeof(InteractObjects))]
[RequireComponent(typeof(CombatSystem))]
public class Controls : MonoBehaviour
{
    public float speedRotation = 10;
    public float speedMovement = 10f;
    public float timerToDragObject = 0.5f;

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
    private GameObject _objInHands;
    private float _timeOfHoldingKey = 0;
    private bool _holdingKey;
    private GameObject _objectBeforeDrag;
    private GameObject _objectToDrag;


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
        if(Input.GetMouseButton(0) && !_objInHands){
            Shooting();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _holdingKey = true;
            if(_interactSystem.CanDrag().collider != null) _objectBeforeDrag = _interactSystem.CanDrag().collider.gameObject;
        } 
        if (Input.GetKeyUp(KeyCode.E))
        {
            _holdingKey = false;
            if (_timeOfHoldingKey < timerToDragObject)
            {
                //Если быстро нажали клавишу и отжали, то взаимодействием с объектом
                Interact();
            }
            else
            {
                //Если долго держали клавишу и отжали, значит держали объект и должны бросить его
                DragObject(false);
            }
            _objectBeforeDrag = null;
        }

        if (_timeOfHoldingKey >= timerToDragObject && !_objInHands && _interactSystem.CanDrag().collider != null)
        {
            //Если долго держим клвашиу, то подбераем предмет
            _objectToDrag = _interactSystem.CanDrag().collider.gameObject;
            if (_objectBeforeDrag == _objectToDrag)
            {
                DragObject(true);
            }
            _objectToDrag = null;
        }

        if(Input.GetKeyDown(KeyCode.Tab)){
            SwitchMap();
        }

        if (_holdingKey) _timeOfHoldingKey += Time.deltaTime;
        else _timeOfHoldingKey = 0;
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
             
             if(hitInteract.collider.tag == "TakableObject"){
                 //TODO Take object
                 Destroy(hitInteract.collider.gameObject);
             }
        }
    }
    
    private void DragObject(bool takeObject){
        if (!takeObject)
        {
            _interactObjects.DropObject(true);
            return;
        }
        RaycastHit hitDrag = _interactSystem.CanDrag();
        if(hitDrag.collider != null){
            if(Vector3.Distance(transform.position, hitDrag.collider.transform.position) > interactDistance) return;
            _interactObjects.TakeObject(hitDrag.collider.gameObject, transform.localScale.y );
            SetObjectInHands(hitDrag.collider.gameObject);
        }
    }

    public void SetObjectInHands(GameObject go)
    {
        _objInHands = go;
    }
}
