using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour {
    // private UnityEvent touchedPlant = default;
    // private UnityEvent died = default;

    public int maxHealth;
    public int attackDamage;
    public float attackTimeout;
    public float speed;
    public float upwardOffset;

    [HideInInspector]
    public bool canMove;

    private int currentHealth;
    private IEnumerator attackRoutine;

    void Start() {
        transform.position += Vector3.up * upwardOffset;
        currentHealth = maxHealth;
        canMove = true;
    }


    void Update() {
        if (canMove)
            Move();
    }

    void Move() {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }

    private IEnumerator Attack(Plant plant) {
        while (true) {
            // play attack animation
            plant.TakeDamage(attackDamage);
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
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Plant")) {
            canMove = false;
            var plant = other.GetComponent<Plant>();
            plant.died.AddListener(() => {
                StopCoroutine(attackRoutine);
                canMove = true;
            });
            attackRoutine = Attack(plant);
            StartCoroutine(attackRoutine);
            // touchedPlant?.Invoke();
        }
    }
}
