using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : MonoBehaviour
{
    public float angleToInspect = 45;
    public float durationOfRotation = 2;
    public float durationOfWait = 0.5f;
    public bool rightRotation;



    private FieldOfViewEnemySystem _fovEnemySystem;
    private Transform _player;
    private Coroutine _inspectCorut;
    private bool _startNextRotation;
    private Quaternion _startRotation;
    


    private bool _playerFind;
    private bool _startRigth;
    private MeshRenderer _meshRenderer; //test
    private Color _startColor; //test

    // Start is called before the first frame update
    void Start()
    {
        _fovEnemySystem = transform.GetComponent<FieldOfViewEnemySystem>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _startColor = _meshRenderer.material.color;
        _startNextRotation = true;
        _startRigth = rightRotation;
        SetStartRotation();
    }

    // Update is called once per frame
    void Update()
    {
        if(_fovEnemySystem._foundPlayer){
            _meshRenderer.material.color = Color.blue;
            _playerFind = true;
            if(_inspectCorut != null){
                StopCoroutine(_inspectCorut);
            }
            RotateToTarget(_player);
        }
        else {
            
            if(_playerFind){
                _inspectCorut = StartCoroutine(BackToStartRotation(durationOfRotation));
                _playerFind = false;
            }

            if(_startNextRotation && rightRotation){
                if(_inspectCorut != null){
                    StopCoroutine(_inspectCorut);
                }
                _inspectCorut = StartCoroutine(Inspect(angleToInspect, durationOfRotation));
            }
            else if (_startNextRotation && !rightRotation){
                if(_inspectCorut != null){
                    StopCoroutine(_inspectCorut);
                }
                _inspectCorut = StartCoroutine(Inspect(-angleToInspect, durationOfRotation));
            }
            
            _meshRenderer.material.color = _startColor;
        }
    }

    private void RotateToTarget(Transform target){
        Vector3 direction = target.position - transform.position;
        direction.Normalize();
        transform.forward = Vector3.Lerp(transform.forward, transform.forward + direction,  _fovEnemySystem.rotateSpeed * Time.deltaTime);
    }

    IEnumerator Inspect(float angle, float duration){

        _startNextRotation = false;

        Quaternion initRot = transform.localRotation;
        
        float timer = 0;

        while(timer < duration){
            timer += Time.deltaTime;
            transform.localRotation = initRot * Quaternion.AngleAxis(timer / duration * angle, Vector3.up);
            yield return null;
        }

        yield return new WaitForSeconds(durationOfWait);

        _startNextRotation = true;
        rightRotation = !rightRotation;
    }

    IEnumerator BackToStartRotation(float duration){

        Quaternion initRot = transform.rotation;

        while(transform.localRotation != _startRotation){

            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, _startRotation, duration * 100 * Time.deltaTime);

            yield return null;
        }

        yield return new WaitForSeconds(durationOfWait);

        _startNextRotation = true;
        rightRotation = _startRigth;
    }


    void SetStartRotation(){
        if(rightRotation){
            transform.localRotation = transform.localRotation * Quaternion.AngleAxis(-angleToInspect / 2, Vector3.up);
        }
        else{
            transform.localRotation = transform.localRotation * Quaternion.AngleAxis(angleToInspect / 2, Vector3.up);
        }

        _startRotation = transform.localRotation;
    }
}
