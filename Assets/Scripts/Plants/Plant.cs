using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Plant : MonoBehaviour {
    // private UnityEvent touchedEnemy = default;
    [HideInInspector]
    public UnityEvent died = default;

    public GameObject bullet;
    public GameObject sun;
    public float upwardOffset;
    public int maxHealth;
    public int attackDamage;
    public int abilityTimeout;
    [SerializeField] PlantType plantType;


    private int currentHealth;
    private System.Action UseAbility;

    void Start() {
        transform.position += Vector3.up * upwardOffset;
        currentHealth = maxHealth;

        UseAbility = plantType switch {
            PlantType.Gun => Attack,
            PlantType.Sun => GenerateSuns,
            _ => () => { }
        };

        StartCoroutine(UseAbilityRoutine());
    }


    void Update() {

    }

    private IEnumerator UseAbilityRoutine() {
        while (true) {
            UseAbility();
            yield return new WaitForSeconds(abilityTimeout);
        }
    }

    private void Attack() {
        // check with raycast first
        var _bullet = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Bullet>();
        _bullet.damage = attackDamage;
    }

    private void GenerateSuns() {
        Instantiate(sun, transform.position + Vector3.up + Random.onUnitSphere * 0.25f, Quaternion.identity);
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

    enum PlantType { Gun, Sun }
}
