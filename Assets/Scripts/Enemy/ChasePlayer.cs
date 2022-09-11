using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(FieldOfViewEnemySystem))]
public class ChasePlayer : MonoBehaviour
{
    public float distanceToPlayer;
    public float updatePlayerPosFrequency;

    public float updateEnemyPosFrequency;

    
    private FieldOfViewEnemySystem _fovEnemySystem;
    private NavMeshAgent _agent;

    private bool _seePlayer;

    
    
    private Transform _player;

    private Vector3 _currentTargetPos;
    private Coroutine _updateCurrentPlayerPos;
    private Coroutine _nextPointCorut;
    private MeshRenderer _meshRenderer; //test
    private Color _startColor; //test

   

    void Start()
    {
        _fovEnemySystem = GetComponent<FieldOfViewEnemySystem>();
        _agent = GetComponent<NavMeshAgent>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _startColor = _meshRenderer.material.color;
        _nextPointCorut = StartCoroutine(UpdateEnemyPos());
        
        
    }

    
    void Update()
    {
        if(_fovEnemySystem._foundPlayer) {
            if(!_seePlayer){
                StopCoroutine(_nextPointCorut);
               _updateCurrentPlayerPos = StartCoroutine(UpdatePlayerPos());
            }
            _seePlayer = true;
        }
        if(!_fovEnemySystem._foundPlayer) {
            if(_seePlayer){
                StopCoroutine(_updateCurrentPlayerPos);
                _nextPointCorut = StartCoroutine(UpdateEnemyPos());
            }
            _seePlayer = false;
        }

        if(_seePlayer) {
            _meshRenderer.material.color = Color.red;
            if(Vector3.Distance(transform.position , _player.position) > distanceToPlayer){
                _agent.SetDestination(_currentTargetPos);
            }
            else if(_fovEnemySystem.CanSeePlayer()) {
                Vector3 direction = transform.position - _player.position;
                direction.Normalize();
                _agent.SetDestination(_player.position + direction*distanceToPlayer);
            }
            RotateToTarget(_player);
        }
        else
        {
            _meshRenderer.material.color = _startColor;
        }

        
    }

    private IEnumerator UpdatePlayerPos(){
        while(true){
            _currentTargetPos = new Vector3(_player.position.x + Random.Range(-distanceToPlayer/2, distanceToPlayer/2), _player.position.y, _player.position.z + Random.Range(-distanceToPlayer/2, distanceToPlayer/2));
            yield return new WaitForSeconds(updatePlayerPosFrequency);
        }
    }

    private IEnumerator UpdateEnemyPos(){

        NavMeshPath newPath = new NavMeshPath();
        NavMeshHit navMeshHit;

        while(true){
            _currentTargetPos = transform.position + new Vector3(Random.Range(-distanceToPlayer*2, distanceToPlayer*2), 0, Random.Range(-distanceToPlayer*2, distanceToPlayer*2));
            NavMesh.SamplePosition(_currentTargetPos, out navMeshHit, distanceToPlayer, NavMesh.AllAreas);
            _currentTargetPos = navMeshHit.position;

            _agent.CalculatePath(_currentTargetPos, newPath);
            if(newPath.status == NavMeshPathStatus.PathComplete){
                _agent.SetDestination(_currentTargetPos);
                yield return new WaitForSeconds(updateEnemyPosFrequency + Random.Range(-updateEnemyPosFrequency / 3, updateEnemyPosFrequency/ 3));
            }
        }
    }

    private void RotateToTarget(Transform target){
        Vector3 direction = target.position - transform.position;
        direction.Normalize();
        transform.forward = Vector3.Lerp(transform.forward, transform.forward + direction, _fovEnemySystem.rotateSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_currentTargetPos, 0.2f);
    }
}
