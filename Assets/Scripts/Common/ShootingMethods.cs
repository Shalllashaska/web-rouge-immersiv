using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingMethods : MonoBehaviour
{

    #region Shooting Types
    
    
    /**
     * Выстрел одной пули типа Hitscan с дальностью выстрела
     * @type 1
     */
    public static void Shoot(
        float distanceShooting,
        float damage,
        float forceStrength,
        Transform attackPoint,
        GameObject particle,
        LayerMask canHitLayer
    )
    {
        RaycastHit hit;
        GameObject impact;
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out hit, distanceShooting, canHitLayer))
        {
            impact = Instantiate(particle, hit.point, Quaternion.identity);
            Damage(hit, damage);
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddForce(attackPoint.forward * forceStrength, ForceMode.Impulse);
            }
        }
        else
        {
            impact = Instantiate
            (particle,
                attackPoint.position + attackPoint.forward * distanceShooting,
                Quaternion.identity
            );
        }
        Destroy(impact, 1.5f);
    }

    /**
     * Выстрел множества пуль типа Hitscan с дальностью выстрела
     * @type 2
     */
    public static void ShotgunShoot(
        float distanceShooting,
        float damage,
        float forceStrength,
        float bloom,
        int mountOfBullets,
        Transform attackPoint,
        GameObject particle,
        LayerMask canHitLayer
    )
    {
        RaycastHit hit;
        GameObject impact;

        for (int i = 0; i < mountOfBullets; i++)
        {
            Vector3 targetPosition = attackPoint.position + attackPoint.forward * distanceShooting;
            targetPosition = new Vector3
            (
                targetPosition.x + Random.Range(-bloom, bloom),
                targetPosition.y,
                targetPosition.z + Random.Range(-bloom, bloom)
            );
            Vector3 direction = (targetPosition - attackPoint.position);
            direction.Normalize();
            if (Physics.Raycast(attackPoint.position, direction, out hit, distanceShooting, canHitLayer))
            {
                impact = GameObject.Instantiate(particle, hit.point, Quaternion.identity);
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                Damage(hit, damage);
                if (rb)
                {
                    rb.AddForce(direction * forceStrength, ForceMode.Impulse);
                }
            }
            else
            {
                impact = Instantiate
                (
                    particle,
                    attackPoint.position + direction * distanceShooting,
                    Quaternion.identity
                );
            }
            Destroy(impact, 1.5f);
        }
    }

    /**
     * Выстрел одной пули типа Hitscan без дальности выстрела
     * @type 3
     */
    public static void LaserShoot(
        float distanceShooting,
        float damage,
        Transform attackPoint,
        GameObject particle,
        LayerMask canHitLayer
    )
    {
        RaycastHit hit;
        GameObject impact;
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out hit, distanceShooting, canHitLayer))
        {
            impact = GameObject.Instantiate(particle, hit.point, Quaternion.identity);
            //Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            //if(rb){
            //    rb.AddForce(transform.forward * forceStrength, ForceMode.Impulse);
            //}
            Damage(hit, damage);
        }
        else
        {
            impact = Instantiate
            (
                particle,
                attackPoint.position + attackPoint.forward * distanceShooting,
                Quaternion.identity
            );
        }
        Destroy(impact, 1.5f);
    }
    
    /**
     * Выстрел одной пули типа Projectail
     * @type 4
     */
    public static void ProjectailShoot(
        float damage,
        float forceStrength,
        Transform attackPoint,
        GameObject projectailGameObject
    )
    {
        GameObject projectail = Instantiate
        (
            projectailGameObject,
            attackPoint.position + attackPoint.forward * 0.8f,
            Quaternion.identity
        );
        Rigidbody rb = projectail.GetComponent<Rigidbody>();
        projectail.GetComponent<ProjectailScript>().SetDamage(damage);
        rb.AddForce(attackPoint.forward * forceStrength, ForceMode.Impulse);
    }

    /**
     * Урон в небольшом поле на все объекты по типу ближней атаки
     * @type 5
     */
    public static void MeleeAttack(
        float distanceShooting,
        int damage,
        float forceStrength,
        float meleeAttackSphereRadius,
        Transform meleeAttackSphere,
        GameObject particle,
        LayerMask canHitLayer
    )
    {
        Collider[] colldersTouch = Physics.OverlapSphere
        (
            meleeAttackSphere.position,
            meleeAttackSphereRadius,
            canHitLayer
        );
        if (colldersTouch.Length > 0)
        {
            foreach (Collider coll in colldersTouch)
            {
                Rigidbody rb = coll.transform.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.AddForce(meleeAttackSphere.forward * forceStrength, ForceMode.Impulse);
                }

                GameObject impact = GameObject.Instantiate(particle, coll.transform.position, Quaternion.identity);
                Destroy(impact, 2f);
            }
        }
    }
    #endregion
    private static void Damage(RaycastHit hit, float damage){
        BodyPart bodyPart = hit.collider.transform.GetComponent<BodyPart>();
        if(bodyPart){
            bodyPart.TakeDamage(damage);
        }
    }
}
