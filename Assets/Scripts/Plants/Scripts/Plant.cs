using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Plant : MonoBehaviour {
    // private UnityEvent touchedEnemy = default;
    [HideInInspector]
    public UnityEvent died = default;

    public PlantType plantType;
    public Sprite icon;

    [Space(), Header("General")]
    public float upwardOffset;
    public int maxHealth;
    public int attackDamage;
    public float abilityTimeout;
    public int cardSpawnTimeout;
    public int price;

    [Header("Shooter")]
    public Bullet bullet;

    [Header("Generator")]
    public GameObject sun;
    public int abilityAmount;

    [Header("Bomb")]
    public float abilityRadius;
    public GameObject explosionParticleSystem;
    [Header("Freeze")]
    public Bullet freezeBullet;
    [Range(0f, 1f)]
    public float speedPercentage;
    public float freezeTime;



    private int currentHealth;
    private System.Action UseAbility;
    private GameManager gameManager;
    private bool isDamaging;
    private Material material;
    private Color color;

    void Start() {
        // transform.position += Vector3.up * upwardOffset;
        currentHealth = maxHealth;
        gameManager = GameManager.instance;
        material = GetComponent<MeshRenderer>().material;
        color = material.color;
        // canUseAbility = true;

        UseAbility = plantType switch {
            PlantType.Melee => Attack,
            PlantType.Gun => Shoot,
            PlantType.Sun => GenerateSuns,
            PlantType.Bomb => Explode,
            PlantType.Freeze => Shoot,
            _ => () => { }
        };

        // StartUseAbility();
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

    private void Die() {
        // play death animation
        died?.Invoke();
        if (gameObject != null)
            Destroy(gameObject);
    }

    private void OnMouseDown() {
        if (gameManager.isDeleting) {
            gameManager.AddCoins(price / 2);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos() {
        if (plantType == PlantType.Bomb || plantType == PlantType.Melee) {
            Gizmos.color = Color.white * 0.33f;
            Gizmos.DrawSphere(transform.position, abilityRadius);
        }
    }

    #region Plants Abilities
    private void Attack() {
        foreach (var collider in Physics.OverlapSphere(transform.position, abilityRadius)) {
            if (!collider.CompareTag("Enemy")) continue;
            collider.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    private void Shoot() {
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
            if (plantType == PlantType.Gun) {
                var _bullet = Instantiate(bullet, transform.position, Quaternion.identity);
                _bullet.damage = attackDamage;
            } else {
                var _bullet = Instantiate(freezeBullet, transform.position, Quaternion.identity);
                _bullet.speedPercentage = speedPercentage;
                _bullet.freezeTime = freezeTime;
            }
        }
    }

    private void GenerateSuns() {
        Instantiate(sun, transform.position + Vector3.up + Random.onUnitSphere * 0.25f, Quaternion.identity).GetComponent<Sun>().coins = abilityAmount;
    }

    private void Explode() {
        Instantiate(explosionParticleSystem, transform.position, Quaternion.identity);
        foreach (var collider in Physics.OverlapSphere(transform.position, abilityRadius)) {
            if (!collider.CompareTag("Enemy")) continue;
            collider.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
        Destroy(gameObject);
    }
    #endregion Plants Abilities

}
public enum PlantType { Melee, Gun, Sun, Bomb, Freeze }
