using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed;
    public bool isFreezeBullet;

    [HideInInspector]
    public int damage;

    [HideInInspector]
    public float speedPercentage;
    [HideInInspector]
    public float freezeTime;

    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.right * speed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            var enemy = other.GetComponent<Enemy>();
            if (isFreezeBullet) {
                enemy.ReduceSpeed(speedPercentage, freezeTime);
            } else {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
