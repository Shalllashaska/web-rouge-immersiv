using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{

    public float speedCameraMovement = 5f;
    public float maxDistanceBetweenPlayerAndCursors = 5;

    private Transform _playerTransform;
    private Transform _cursorTransform;
    private Camera _mainCamera;

    private Vector3 _mouseDirection;
    private Vector3 _mousePosition;
    private Vector3 _cameraPositon;
    private Vector3 _currentCameraPosition;
    private float _distanceBetweenMousePositionAndPlayer;

    private float _startDistance;
    private float _startSize;
    private float _currentSize;

    private float _startFOV;
    private float _currentFOV;
    private float _currentDistance;
    private float _startPositionYCameraHandler;
    private bool _isOrtographic;

    void Start()
    {
        _startDistance = maxDistanceBetweenPlayerAndCursors;
        _playerTransform = GameObject.Find("Player").transform;
        _mainCamera = Camera.main;
        _startSize = Camera.main.orthographicSize;
        _startFOV = Camera.main.fieldOfView;
        _currentSize = _startSize;
        _currentFOV = _startFOV;
        _isOrtographic = _mainCamera.orthographic;
        _startPositionYCameraHandler = transform.position.y;
    }


    void Update()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            maxDistanceBetweenPlayerAndCursors = _startDistance * 2;
            float curSize = _startSize + _currentDistance / 2;
            float curFOV = _startFOV + _currentDistance / 2;
            _currentSize = Mathf.Lerp(_currentSize, curSize, Time.deltaTime * speedCameraMovement);
            _currentFOV = Mathf.Lerp(_currentFOV, curFOV, Time.deltaTime * speedCameraMovement);
        }
        else
        {
            maxDistanceBetweenPlayerAndCursors = _startDistance;
            float curSize = _startSize;
            float curFOV = _startFOV;
            _currentSize = Mathf.Lerp(_currentSize, curSize, Time.deltaTime * speedCameraMovement);
            _currentFOV = Mathf.Lerp(_currentFOV, curFOV, Time.deltaTime * speedCameraMovement);
        }


    }

    private void FixedUpdate()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        _mousePosition = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.transform.position.y));
        _mouseDirection = (_mousePosition - _playerTransform.position).normalized;
        _distanceBetweenMousePositionAndPlayer = Vector3.Distance(_mousePosition, _playerTransform.position);


        if (_distanceBetweenMousePositionAndPlayer >= maxDistanceBetweenPlayerAndCursors)
        {
            _cameraPositon = _playerTransform.position + _mouseDirection * (maxDistanceBetweenPlayerAndCursors / 2);
            _currentDistance = maxDistanceBetweenPlayerAndCursors / 2;
        }
        else
        {
            _cameraPositon = _playerTransform.position + _mouseDirection * (_distanceBetweenMousePositionAndPlayer / 2);
            _currentDistance = _distanceBetweenMousePositionAndPlayer / 2;
        }

        if (_isOrtographic)
        {
            _mainCamera.orthographicSize = _currentSize;
        }
        else
        {
            _mainCamera.fieldOfView = _currentFOV;
        }
        _currentCameraPosition = Vector3.Lerp(new Vector3(_currentCameraPosition.x, _startPositionYCameraHandler, _currentCameraPosition.z), _cameraPositon, Time.fixedDeltaTime * speedCameraMovement);
        transform.position = _currentCameraPosition;
    }
}
