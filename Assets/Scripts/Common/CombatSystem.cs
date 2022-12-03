using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    
    [SerializeField] private LayerMask canHitLayer;

    private WeaponData _currentWeaponData;
    private float _currentDamage;
    private float _currentCooldown;
    private float _currentTimerCooldown;
    private int _currentMountOfBullets;
    private float _currentBloom;
    private float _currentMeleeAttackSphereRadius;
    private Transform _currentMeleeAttackSphere;
    private GameObject _currentImpact;
    private GameObject _currentProjectail;
    private float _currentForceStrength;
    private float _currentDistanceShooting;


    private void Start()
    {
        _updateMeleeAttackSphere();
    }

    public void Update(){
        if(_currentTimerCooldown >= 0){
            _currentTimerCooldown -= Time.deltaTime;
        }
    }

    public void Shoot(){
        if(_currentTimerCooldown > 0) return;
        
        _currentTimerCooldown = _currentCooldown;
    }

    private void _updateMeleeAttackSphere()
    {
        if (_currentMeleeAttackSphere)
        {
            _currentMeleeAttackSphereRadius = _currentMeleeAttackSphere.GetComponent<SphereCollider>().radius;
        }
    }

    private void _updateAllWeaponData()
    {
        _currentDamage = _currentWeaponData.Damage;
        _currentCooldown = _currentWeaponData.Cooldown;
        _currentTimerCooldown = _currentWeaponData.TimerCooldown;
        _currentMountOfBullets = _currentWeaponData.MountOfBulletsInOneShot;
        _currentBloom = _currentWeaponData.Bloom;
        _currentMeleeAttackSphere = _currentWeaponData.MeleeAttackSphere;
        _currentMeleeAttackSphereRadius = _currentMeleeAttackSphere.GetComponent<SphereCollider>().radius;
        _currentImpact = _currentWeaponData.Impact;
        _currentProjectail = _currentWeaponData.Projectail;
        _currentForceStrength = _currentWeaponData.ForceStrength;
        _currentDistanceShooting = _currentWeaponData.DistanceShooting;
    }
}

//TODO Реализовать подъем оружия используя методы которые написал в скриптах WeaponData, WeaponMaster, ShootingMethods
