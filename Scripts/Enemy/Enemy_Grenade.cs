using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Grenade : MonoBehaviour
{
    [SerializeField] private GameObject explosionFx;
    [SerializeField] private float impactRadius;
    [SerializeField] private float upwardsMultiplier = 1;
    private Rigidbody rb;
    private float timer;
    private float impactPower;
    public int grenadeDamage = 30;  

    private void Awake() => rb = GetComponent<Rigidbody>();

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
            Explode();
    }

    private void Explode()
    {
        GameObject newFx = ObjectPool.instance.GetObject(explosionFx, transform);
        ObjectPool.instance.ReturnObject(newFx, 1);
        ObjectPool.instance.ReturnObject(gameObject);

        // Find all objects within the impact radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, impactRadius);

        foreach (Collider hit in colliders)
        {
            // Check if the object has a tag "NoDamage" and skip it if true
            if (hit.CompareTag("Guns"))
            {
                continue;
            }

            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(impactPower, transform.position, impactRadius, upwardsMultiplier, ForceMode.Impulse);

            // Check for PlayerHealth component and apply damage if player is hit
            Player player = hit.GetComponentInParent<Player>();
            if (player != null)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(grenadeDamage);  // Apply damage to player
                    Debug.Log("Grenade hit the player!");
                }
            }
        }
    }

    public void SetupGrenade(Vector3 target, float timeToTarget, float countdown, float impactPower)
    {
        rb.linearVelocity = CalculateLaunchVelocity(target, timeToTarget);
        timer = countdown + timeToTarget;
        this.impactPower = impactPower;
    }

    private Vector3 CalculateLaunchVelocity(Vector3 target, float timeToTarget)
    {
        Vector3 direction = target - transform.position;
        Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);

        Vector3 velocityXZ = directionXZ / timeToTarget;

        float velocityY =
            (direction.y - (Physics.gravity.y * Mathf.Pow(timeToTarget, 2)) / 2) / timeToTarget;

        Vector3 launchVelocity = velocityXZ + Vector3.up * velocityY;

        return launchVelocity;
    }

    private void OnDrawGizmos()
    {
        // Set the Gizmos color to a transparent yellow for better visualization
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f); // Semi-transparent yellow
        Gizmos.DrawSphere(transform.position, impactRadius); // Filled sphere

        // Set the Gizmos color to solid yellow for the wireframe
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, impactRadius); // Wire sphere
    }
}
