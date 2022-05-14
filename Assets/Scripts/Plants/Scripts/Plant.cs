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
    public float abilityTimeout;
    public float abilityRadius;
    public int abilityAmount;
    public int price;

    public Sprite icon;
    // [HideInInspector] public bool canUseAbility;
    [SerializeField] public PlantType plantType;


    private int currentHealth;
    private System.Action UseAbility;

    void Start() {
        // transform.position += Vector3.up * upwardOffset;
        currentHealth = maxHealth;
        // canUseAbility = true;

        UseAbility = plantType switch {
            PlantType.Gun => Attack,
            PlantType.Sun => GenerateSuns,
            PlantType.Bomb => Explode,
            _ => () => { }
        };

        // StartUseAbility();
    }


    void Update() {

    }

    public void StartUseAbility() {
        StartCoroutine(UseAbilityRoutine());
    }

    private IEnumerator UseAbilityRoutine() {
        while (true) {
            // if (canUseAbility)
            UseAbility?.Invoke();
            yield return new WaitForSeconds(abilityTimeout);
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
        if (gameObject != null)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            // touchedEnemy?.Invoke();
        }
    }

    private void OnDrawGizmos() {
        if (plantType == PlantType.Bomb) {
            Gizmos.color = Color.white * 0.33f;
            Gizmos.DrawSphere(transform.position, abilityRadius);
        }
    }

    #region Plants Abilities
    private void Attack() {
        // check with raycast first
        var enemyExist = false;
        // foreach (var collider in Physics.RaycastAll(transform.position, Vector3.right)) {

        foreach (var collider in Physics.BoxCastAll(transform.position, Vector3.one * 0.25f, Vector3.right)) {
            if (collider.collider.CompareTag("Enemy")) {
                enemyExist = true;
                break;
            }
        }

        if (enemyExist) {
            var _bullet = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Bullet>();
            _bullet.damage = attackDamage;
        }
    }

    private void GenerateSuns() {
        Instantiate(sun, transform.position + Vector3.up + Random.onUnitSphere * 0.25f, Quaternion.identity).GetComponent<Sun>().coins = abilityAmount;
    }

    private void Explode() {
        foreach (var collider in Physics.OverlapSphere(transform.position, abilityRadius)) {
            if (!collider.CompareTag("Enemy")) continue;
            collider.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
        Destroy(gameObject);
    }
    #endregion Plants Abilities

}
public enum PlantType { Gun, Sun, Bomb }
