public class EnemyHealth : Health {
    public float deathAnimationTime = 1.6f;
    
    public override void TakeDamage(int damage) {
        base.TakeDamage(damage);
        if (health > 0) AudioManager.instance.Play("damage");
        if (health <= 0) {
            //spawn particles
            //play death sound
            AudioManager.instance.Play("damage");
            Destroy(gameObject, deathAnimationTime);
        }
    }
    
}