using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellContact : MonoBehaviour
{
    public LayerMask tankMask;
    public float MaxDamage = 100f;
    public float explosionForce = 1000f;
    public float explosionRadius = 5f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, tankMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            if (!targetRigidbody)
                continue;

            targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            Tank_Data targetHealth = targetRigidbody.GetComponent<Tank_Data>();

            if (!targetHealth)
                continue;

            float damage = CalculateDamage(targetRigidbody.position);

            targetHealth.TakeDamage(damage);
        }
    }

    private float CalculateDamage (Vector3 targetPosition)
    {
        return 0f;
    }

}
