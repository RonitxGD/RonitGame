using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : Bullet
{
    public int damage = 10;

    protected override void OnCollisionEnter(Collision collision)
    {
        CreateImpactFx();
        ReturnBulletToPool();

        Player player = collision.gameObject.GetComponentInParent<Player>();

        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Damage the player
                Debug.Log("Enemy bullet hit the Player!");

              
            }
        }
    }
}
