using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Axe : MonoBehaviour
{
    [SerializeField] private GameObject impactFx;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform axeVisual;

    private Vector3 direction;
    private Transform player;
    private float flySpeed;
    private float rotationSpeed;
    private float timer = 1;

    public int axeDamage = 20;  

    public void AxeSetup(float flySpeed, Transform player, float timer)
    {
        rotationSpeed = 1600;

        this.flySpeed = flySpeed;
        this.player = player;
        this.timer = timer;
    }

    private void Update()
    {
        axeVisual.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        timer -= Time.deltaTime;

        if (timer > 0)
            direction = player.position + Vector3.up - transform.position;

        rb.linearVelocity = direction.normalized * flySpeed;
        transform.forward = rb.linearVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        Player player = other.GetComponent<Player>();

        if (bullet != null || player != null)
        {
            // Apply damage to the player if hit
            if (player != null)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(axeDamage);  // Apply damage to the player
                    Debug.Log("Axe hit the player!");
                }
            }

            // Handle impact effects and object pooling
            GameObject newFx = ObjectPool.instance.GetObject(impactFx, transform);
            ObjectPool.instance.ReturnObject(gameObject);
            ObjectPool.instance.ReturnObject(newFx, 1f);
        }
    }
}
