using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractObjects : MonoBehaviour
{
    public float throwForce = 5f;
    public float dragSpeed = 10f;
    public float draggableDistanceInsideObjects = 5f;
    public float draggableDistance = 5f;
    private Camera _mainCamera;

    private Vector3 _mousePosition;
    private GameObject _objectInHands;
    private Rigidbody _rigidbodyInHands;
    private Transform _currentPositionObject;
    private float _yPosition;
    private Vector3 _supposedPosition;
    private float _distanceBetweenCurrentAndSupposedPosition;
    private Vector3 _lastAvailablePoint;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!_objectInHands) return;
        _mousePosition = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.transform.position.y));
        Vector3 point = new Vector3(_mousePosition.x, _yPosition, _mousePosition.z);
        if (Vector3.Distance(transform.position, point) >= draggableDistance)
        {
            Vector3 directionNormalized = (point - transform.position).normalized;
            _supposedPosition = transform.position + directionNormalized * draggableDistance;
        }
        else
        {
            _supposedPosition = point;
        }
    }

    private void FixedUpdate()
    {
        if (!_objectInHands) return;
        Vector3 newPos = Vector3.Lerp(
            _objectInHands.transform.position,
            _lastAvailablePoint == Vector3.zero ? _supposedPosition : _lastAvailablePoint,
            Time.fixedDeltaTime * dragSpeed
            );
        if (!_CanTranslateObject() && _distanceBetweenCurrentAndSupposedPosition > draggableDistanceInsideObjects)
        {
            DropObject(false);
        }
        _rigidbodyInHands.MovePosition(newPos);
    }

    public void TakeObject(GameObject obj, float yPos)
    {
        _objectInHands = obj;
        _yPosition = yPos + obj.transform.position.y;
        _supposedPosition = new Vector3(_objectInHands.transform.position.x, _yPosition,
            _objectInHands.transform.position.z);
        _rigidbodyInHands = _objectInHands.GetComponent<Rigidbody>();
        _rigidbodyInHands.useGravity = false;
        _distanceBetweenCurrentAndSupposedPosition = 0;
    }
    
    public void DropObject(bool isThrowObject)
    {
        if (!_objectInHands) return;
        if (isThrowObject)
        {
            Vector3 endPoint;
            endPoint = _mousePosition;
            if (Vector3.Distance(transform.position, _mousePosition) >= draggableDistance * 2)
            {
                Vector3 directionNormalized = (_mousePosition - transform.position).normalized;
                endPoint = transform.position + directionNormalized * draggableDistance * 2;
            }
            Vector3 differenceVector = endPoint - _objectInHands.transform.position ;
            float mult = 1;
            Debug.Log("dist " + Vector3.Distance(_objectInHands.transform.position, transform.position));
            Debug.Log("pos " + _objectInHands.transform.position);
            if (Vector3.Distance(_objectInHands.transform.position, transform.position) < draggableDistance - 0.5f)
            {
                mult = 2;
            }
            _rigidbodyInHands.AddForce(differenceVector * throwForce * mult, ForceMode.Impulse);
        }
        _objectInHands = null;
        _rigidbodyInHands.useGravity = true;
    }
    
    private bool _CanTranslateObject()
    {
        RaycastHit hit;
        Vector3 curPos = _objectInHands.transform.position;
        Ray ray = new Ray(curPos, _supposedPosition - curPos);
        _distanceBetweenCurrentAndSupposedPosition = Vector3.Distance(curPos, _supposedPosition);
        if (Physics.Raycast(ray, out hit, _distanceBetweenCurrentAndSupposedPosition))
        {
            _lastAvailablePoint = hit.point;
            return false;
        }
        _lastAvailablePoint = Vector3.zero;
        return true;
    } 
}
