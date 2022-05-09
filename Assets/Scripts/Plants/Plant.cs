using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Plant : MonoBehaviour {
    // private UnityEvent touchedEnemy = default;
    [HideInInspector]
    public UnityEvent died = default;

    public GameObject bullet;
    public float upwardOffset;
    public int maxHealth;
    public int attackDamage;
    public int attackTimeout;


    private int currentHealth;

    void Start() {
        transform.position += Vector3.up * upwardOffset;
        currentHealth = maxHealth;

        StartCoroutine(Attack());
    }


    void Update() {

    }

    private IEnumerator Attack() {
        while (true) {
            var _bullet = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Bullet>();
            _bullet.damage = attackDamage;
            yield return new WaitForSeconds(attackTimeout);
        }
    }

    public void TakeDamage(int damage) {
        // play hit animation
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        // play death animation
        died?.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            // touchedEnemy?.Invoke();
        }
    }
}
