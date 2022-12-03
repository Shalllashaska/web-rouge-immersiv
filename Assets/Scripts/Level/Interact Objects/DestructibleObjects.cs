using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro.EditorUtilities;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Rigidbody))]
public class DestructibleObjects : MonoBehaviour
{
    public float maxVelocityMagnitudeForTakeDamage = 5;
    public GameObject inpactDestroy;
    private bool _startDetectCollisions = false;
    private Health _health;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _health = gameObject.GetComponent<Health>();
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    public void StartDetectCollisions()
    {
        if (_startDetectCollisions) return;
        _startDetectCollisions = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!_startDetectCollisions) return;

        if (_rigidbody.velocity.magnitude >= maxVelocityMagnitudeForTakeDamage)
        {
            if (_health.GetHealth() - (_rigidbody.velocity.magnitude - maxVelocityMagnitudeForTakeDamage) <= 0)
            {
                Instantiate(inpactDestroy, transform.position, transform.rotation);
            }
            _health.TakeDamage(_rigidbody.velocity.magnitude - maxVelocityMagnitudeForTakeDamage);
        }
    }
    
}
