using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectailScript : MonoBehaviour
{   
    public GameObject inpactParticle;
    public LayerMask canHit;

    private float _damage;

    public void SetDamage(float d){
        _damage = d;
    }

    public void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "Player"){
            return;
        }

        if(other.gameObject.tag == "Enemy"){
            Debug.Log("Enemy!");
            other.gameObject.GetComponent<BodyPart>().TakeDamage(_damage);
        }
        
        GameObject inpact = Instantiate(inpactParticle, transform.position, Quaternion.identity);
        Destroy(inpact, 5f);
        Destroy(gameObject);
    }
}
