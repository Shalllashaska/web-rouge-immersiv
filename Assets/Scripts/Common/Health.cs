using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    private float _health;
    private float _currentHealth;
    private bool _dead;

    public void InitHealth(int h){
        _health = h;
        _currentHealth = _health;
        _dead = false;
    }

    public void TakeDamage(float d){
        _currentHealth -= d;
        Debug.Log(_currentHealth);
        if(_currentHealth <= 0){
            _dead = true;
            Destroy(gameObject);
        }
    }

    
   
}
