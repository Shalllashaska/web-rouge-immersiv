using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public float multiplierOfDamage = 1;
    private Health _mainHealth;
    private bool init = false;

    public void InitPart(Health h){
        _mainHealth =h;
        init = true;
    }

    public void TakeDamage(float damage){
        _mainHealth.TakeDamage(damage * multiplierOfDamage);
    }

   
}
