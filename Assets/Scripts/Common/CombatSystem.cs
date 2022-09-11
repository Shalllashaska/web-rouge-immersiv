using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public GameObject particle;
    public Transform meleeAttackSphere;
    public GameObject _currentProjectail;
    public float _currentStrengthOfPushProjectail = 100;
    public float distanceShooting;
    public float forceStrength;
    public int _currentDamage;

    public LayerMask canHitLayer;

    private float _currentCooldown = 0.5f;
    private float _currentTimerCooldown;
    private int _currentMountOfBullets = 10;
    private float _currentBloom = 5f;
    private float _meleeAttackSphereRadius;


    private void Start(){
        if(meleeAttackSphere){
            _meleeAttackSphereRadius = meleeAttackSphere.GetComponent<SphereCollider>().radius;
        }
    }

    public void Update(){
        if(_currentTimerCooldown >= 0){
            _currentTimerCooldown -= Time.deltaTime;
        }
    }

    public void Shoot(){
        if(_currentTimerCooldown > 0) return;

        RaycastHit hit;
        GameObject impact;

        if(Physics.Raycast(transform.position, transform.forward, out hit, distanceShooting, canHitLayer)){
            impact = GameObject.Instantiate(particle, hit.point, Quaternion.identity);

            Damage(hit, _currentDamage);

            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if(rb){
                rb.AddForce(transform.forward * forceStrength, ForceMode.Impulse);
            }

            
        }
        else{
            impact = GameObject.Instantiate(particle, transform.position + transform.forward * distanceShooting, Quaternion.identity);
        }

        Destroy(impact, 1.5f);
        _currentTimerCooldown = _currentCooldown;
    }

    public void ShotgunShoot(){
        if(_currentTimerCooldown > 0) return;

        RaycastHit hit;
        GameObject impact;

        for(int i = 0; i < _currentMountOfBullets; i++){
            Vector3 targetPosition = transform.position + transform.forward * distanceShooting;
            targetPosition = new Vector3(
                targetPosition.x + Random.Range(-_currentBloom, _currentBloom),
                targetPosition.y,
                targetPosition.z + Random.Range(-_currentBloom, _currentBloom)
            );

            Vector3 direction = (targetPosition - transform.position);
            direction.Normalize();

            if(Physics.Raycast(transform.position, direction, out hit, distanceShooting, canHitLayer)){
            impact = GameObject.Instantiate(particle, hit.point, Quaternion.identity);

            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

            Damage(hit, _currentDamage);
            if(rb){
                rb.AddForce(direction * forceStrength, ForceMode.Impulse);
            }
            }
            else{
            impact = GameObject.Instantiate(particle, transform.position + direction * distanceShooting, Quaternion.identity);
            }

        Destroy(impact, 1.5f);
        }

        
        _currentTimerCooldown = _currentCooldown;
    }

    private void Damage(RaycastHit hit, int damage){

        BodyPart bodyPart = hit.collider.transform.GetComponent<BodyPart>();
        if(bodyPart){
            bodyPart.TakeDamage(damage);
        }
    }

    public void LaserShoot(){

        RaycastHit hit;
        GameObject impact;

        if(Physics.Raycast(transform.position, transform.forward, out hit, distanceShooting, canHitLayer)){
            impact = GameObject.Instantiate(particle, hit.point, Quaternion.identity);
            //Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            //if(rb){
            //    rb.AddForce(transform.forward * forceStrength, ForceMode.Impulse);
            //}

            Damage(hit, _currentDamage);
        }
        else{
            impact = GameObject.Instantiate(particle, transform.position + transform.forward * distanceShooting, Quaternion.identity);
        }

        Destroy(impact, 1.5f);
    }

    public void ProjectailShoot(){

        if(_currentTimerCooldown > 0) return;

        GameObject projectail = GameObject.Instantiate(_currentProjectail, transform.position + transform.forward * 0.8f, Quaternion.identity);
        Rigidbody rb = projectail.GetComponent<Rigidbody>();

        projectail.GetComponent<ProjectailScript>().SetDamage(_currentDamage);
        

        rb.AddForce(transform.forward * _currentStrengthOfPushProjectail, ForceMode.Impulse);
        
        _currentTimerCooldown = _currentCooldown;
    }

    public void MeleeAttack(){
        if(_currentTimerCooldown > 0) return;

        Collider[] colldersTouch = Physics.OverlapSphere(meleeAttackSphere.position, _meleeAttackSphereRadius, canHitLayer);

        if(colldersTouch.Length > 0) {

        
        foreach (Collider coll in colldersTouch)
        {
            Rigidbody rb = coll.transform.GetComponent<Rigidbody>();
            if(rb){
                rb.AddForce(transform.forward * forceStrength, ForceMode.Impulse);
            }
            
            GameObject impact = GameObject.Instantiate(particle, coll.transform.position, Quaternion.identity);
            Destroy(impact, 2f);
        }
        }
        
        _currentTimerCooldown = _currentCooldown;
    }
    
}
