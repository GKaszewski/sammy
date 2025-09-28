public class EnemyHealth : Health {
    public float deathAnimationTime = 1.6f;
    
    public override void TakeDamage(int damage) {
        base.TakeDamage(damage);
        if (health > 0) AudioManager.Instance.Play("damage");
        if (health <= 0) {
            //spawn particles
            //play death sound
            AudioManager.Instance.Play("damage");
            Destroy(gameObject, deathAnimationTime);
        }
    }
    
}