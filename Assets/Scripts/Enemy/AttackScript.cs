using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{

    public float timeBetweenAttack;
    private CombatSystem _cbtSystem;
    private bool startAttacking = false;

    private float _currentTimer = 0;


    void Start()
    {
        _cbtSystem = GetComponent<CombatSystem>();
    }


    private void Update(){
        if(!startAttacking) return;
        if(_currentTimer <= 0){
            _cbtSystem.Shoot();
            _currentTimer = timeBetweenAttack;
        }
        else{
            _currentTimer -= Time.deltaTime;
        }
    }

    public void StartAttacking(){
        startAttacking = true;
    }

    public void StopAttacking(){
        startAttacking = false;
    }
}
