using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Macros;
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
    public Transform shootPoint;

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
    private List<Material> materials;
    private Animator animator;
    private float attackAnimationTimeout;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    void Start() {
        // transform.position += Vector3.up * upwardOffset;
        currentHealth = maxHealth;
        gameManager = GameManager.instance;
        shootPoint = transform.Find("ShootPoint");
        materials = GetComponentsInChildren<Renderer>().Select(r => r.material).ToList();
        attackAnimationTimeout = (plantType == PlantType.Gun || plantType == PlantType.Freeze) ? 0.35f : (plantType == PlantType.Melee) ? 1 : 0;
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
        yield return new WaitForSeconds(plantType == PlantType.Bomb ? 0 : 1);
        while (true) {
            // if (canUseAbility)
            if (plantType == PlantType.Melee && CheckForEnemyMelee()) {
                if (animator != null)
                    animator.SetTrigger("Swing");
            } else if ((plantType == PlantType.Gun || plantType == PlantType.Freeze) && EnemyExist()) {
                if (animator != null)
                    animator.SetTrigger("Shoot");
            }
            yield return new WaitForSeconds(attackAnimationTimeout);
            UseAbility?.Invoke();
            yield return new WaitForSeconds(abilityTimeout);
        }
    }

    public void TakeDamage(int damage) {
        if (plantType == PlantType.House) {
            HandleHouseAttack(damage);
            return;
        }
        SoundManager.instance.PlaySound(Sounds.PlantHit);
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

        var colors = new List<Color>();
        // animator.SetTrigger("Hit");
        foreach (var material in materials) {
            colors.Add(material.color);
            material.color = Color.red;
        }

        yield return new WaitForSeconds(0.2f);

        foreach (var material in materials) {
            material.color = colors[0];
            colors.RemoveAt(0);
        }

        isDamaging = false;

    }

    private void Die(bool instant = false) {
        // play death animation
        died?.Invoke();
        StopAllCoroutines();
        UseAbility = () => { };
        GetComponentsInChildren<Collider>().ToList().ForEach(c => c.enabled = false);
        if (animator != null)
            animator.SetTrigger("Dead");
        if (gameObject != null)
            Destroy(gameObject, (instant || plantType == PlantType.Sun) ? 0 : 3);
    }

    private void OnMouseDown() {
        if (gameManager.isDeleting) {
            gameManager.AddCoins(price / 2);
            // died?.Invoke();
            // Destroy(gameObject);
            Die(true);
        }
    }

    private bool EnemyExist() {
        var enemyExist = false;
        foreach (var collider in Physics.BoxCastAll(transform.position, Vector3.one * 0.25f, Vector3.right)) {
            if (collider.collider.CompareTag("Enemy")) {
                enemyExist = true;
                break;
            }
        }
        return enemyExist;
    }

    private bool CheckForEnemyMelee() {
        var enemyExist = false;
        foreach (var collider in Physics.OverlapSphere(transform.position, abilityRadius)) {
            if (collider.CompareTag("Enemy")) {
                enemyExist = true;
                break;
            }
        }
        return enemyExist;
    }

    private void OnDrawGizmos() {
        if (plantType == PlantType.Bomb || plantType == PlantType.Melee) {
            Gizmos.color = Color.black * 0.33f;
            Gizmos.DrawSphere(transform.position, abilityRadius);
        }
    }

    #region Plants Abilities
    private void Attack() {
        foreach (var collider in Physics.OverlapSphere(transform.position, abilityRadius)) {
            if (!collider.CompareTag("Enemy")) continue;
            // animator.SetTrigger("Attack");
            collider.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
        SoundManager.instance.PlaySound(Sounds.Sword);
    }

    private void Shoot() {
        if (EnemyExist()) {
            // animator.SetTrigger("Shoot");
            SoundManager.instance.PlaySound(Sounds.Gun);
            if (plantType == PlantType.Gun) {
                var _bullet = Instantiate(bullet, shootPoint.position, Quaternion.identity);
                _bullet.damage = attackDamage;
            } else {
                var _bullet = Instantiate(freezeBullet, shootPoint.position, Quaternion.identity);
                _bullet.speedPercentage = speedPercentage;
                _bullet.freezeTime = freezeTime;
            }
        }
    }

    private void GenerateSuns() {
        Instantiate(sun, transform.position + Vector3.up + Random.onUnitSphere * 0.25f, Quaternion.identity).GetComponent<Sun>().coins = abilityAmount;
        SoundManager.instance.PlaySound(Sounds.Coin);
    }

    private void Explode() {
        Instantiate(explosionParticleSystem, transform.position, Quaternion.identity);
        foreach (var collider in Physics.OverlapSphere(transform.position, abilityRadius)) {
            if (!collider.CompareTag("Enemy")) continue;
            collider.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
        // died?.Invoke();
        // Destroy(gameObject);
        SoundManager.instance.PlaySound(Sounds.Bomb);
        Die(true);
    }

    private void HandleHouseAttack(int amount) {
        gameManager.AttackHouse(amount);
    }
    #endregion Plants Abilities

}
public enum PlantType { Melee, Gun, Sun, Bomb, Freeze, House }
