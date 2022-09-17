using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewEnemySystem : MonoBehaviour
{

    public LayerMask obstaclesMask;
    [Range(0, 360f)]
    public float viewAngle;

    [Range(0, 200f)]
    public float viewDistance;

    public float rotateSpeed;

    public float timeToLose;
    public float timeToSpot;


    private EnemyMaster _enemyMaster;
    private Transform _player;
    private float _currentTimerToLose;
    private float _currentTimerToSpot;
    private AttackScript _attackScript;

    public bool _foundPlayer;

    private Color _startColor; //Test

    
    void Start()
    { 
        _enemyMaster = transform.parent.GetComponent<EnemyMaster>();
        _attackScript = gameObject.GetComponent<AttackScript>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _currentTimerToLose = timeToLose;
        _currentTimerToSpot = 0;
    }

    void Update(){

        if(_currentTimerToSpot >= timeToSpot) {
           PlayerFound();
        }
        if(_currentTimerToLose >=  timeToLose) {
            PlayerLoose();
        }



        if(CanSeePlayer()){
            if(_currentTimerToSpot < timeToSpot) _currentTimerToSpot += Time.deltaTime;
            _currentTimerToLose = 0;
        }
        else if(_currentTimerToLose < timeToLose){
            _currentTimerToLose += Time.deltaTime;
            if(_currentTimerToSpot > 0 ) _currentTimerToSpot -= Time.deltaTime;
        }

    }
    public bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, _player.position) <= viewDistance)
        {
            Vector3 dirToPlayer = (_player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer <= viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, _player.position,  obstaclesMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void SetStateFoundPlayer(){
        _foundPlayer = true;
        _currentTimerToLose = 0;
    }

    private void PlayerFound(){
        _foundPlayer = true;
        _currentTimerToLose = 0;
        _enemyMaster.PlayerFound();
        _attackScript.StartAttacking();
    }

    private void PlayerLoose(){
        _foundPlayer = false;
        _currentTimerToSpot = 0;
        _attackScript.StopAttacking();
        _enemyMaster.PlayerLoose();
    }

    private void OnDrawGizmos(){
        if(_player){
            if(CanSeePlayer()){
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, _player.position);
            }
            else{
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, _player.position);
            }
        }
    }


    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
		if (!angleIsGlobal) {
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

}
