using System;
using UnityEngine;

public class EnemyHealth : Health {
    public override void TakeDamage(int damage) {
        base.TakeDamage(damage);
        if (health > 0) AudioManager.instance.Play("damage");
        if (health <= 0) {
            //spawn particles
            //play death sound
            //maybe spawn ragdoll or play death animation
            Destroy(gameObject);
        }
    }
    
}