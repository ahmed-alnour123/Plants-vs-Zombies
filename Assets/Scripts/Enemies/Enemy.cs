using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour {
    // private UnityEvent touchedPlant = default;
    [HideInInspector]
    public UnityEvent died = default;

    public int maxHealth;
    public int attackDamage;
    public float attackTimeout;
    public float speed;
    public float upwardOffset;
    public GameObject smokeParticleSystem;

    [HideInInspector]
    public bool canMove;

    private int currentHealth;
    private IEnumerator attackRoutine;
    private bool isDamaging;
    private bool isSlowing;
    private Material material;
    private Color color;

    void Start() {
        transform.position += Vector3.up * upwardOffset;
        currentHealth = maxHealth;
        canMove = true;
        material = GetComponentInChildren<MeshRenderer>().material;
        color = material.color;
    }

    void Update() {
        if (canMove)
            Move();
    }

    void Move() {
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
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
        } else {
            StartCoroutine(DamageEffect());
        }
    }

    IEnumerator DamageEffect() {

        if (isDamaging) yield break;

        isDamaging = true;

        material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        material.color = color;

        isDamaging = false;


    }

    public void ReduceSpeed(float percentage, float time) {
        if (!isSlowing)
            StartCoroutine(ReduceSpeedRoutine(percentage, time));
    }

    IEnumerator ReduceSpeedRoutine(float percentage, float time) {
        isSlowing = true;
        var originalSpeed = speed;
        speed *= percentage;
        yield return new WaitForSeconds(time);
        speed = originalSpeed;
        isSlowing = false;
    }

    private void Die() {
        died?.Invoke();
        Instantiate(smokeParticleSystem, transform.position, Quaternion.identity);
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
