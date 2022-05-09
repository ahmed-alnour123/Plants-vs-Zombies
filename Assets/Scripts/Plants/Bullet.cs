using System.Linq;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed;

    [HideInInspector]
    public int damage;

    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.forward * speed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            other.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}