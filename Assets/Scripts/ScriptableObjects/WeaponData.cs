using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponData : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private int typeOfShot;
    [SerializeField] private int mountOfBulletsInOneShot;
    [SerializeField] private int mountMaxAmmoInMagazine;
    [SerializeField] private float damage;
    [SerializeField] private float cooldown;
    [SerializeField] private float timerCooldown;
    [SerializeField] private float bloom;
    [SerializeField] private float forceStrength;
    [SerializeField] private float distanceShooting;
    [SerializeField] private GameObject impact;
    [SerializeField] private GameObject projectail;
    
    [SerializeField] private Transform meleeAttackSphere;

    public string Name => name;
    public string Description => description;
    public int TypeOfShot => typeOfShot;
    public float Damage => damage;
    public float Cooldown => cooldown;
    public float TimerCooldown => timerCooldown;
    public int MountOfBulletsInOneShot => mountOfBulletsInOneShot;
    public float Bloom => bloom;
    public GameObject Impact => impact;
    public GameObject Projectail => projectail;
    public float ForceStrength => forceStrength;
    public float DistanceShooting => distanceShooting;
    public Transform MeleeAttackSphere => meleeAttackSphere;
    public int MountMaxAmmoInMagazine => mountMaxAmmoInMagazine;
}