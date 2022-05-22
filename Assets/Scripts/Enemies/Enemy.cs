using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour {
    // private UnityEvent touchedPlant = default;
    [HideInInspector]
    public UnityEvent died = default;

    public int maxHealth;
    public int attackDamage;
    [HideInInspector]
    public float attackAnimationTimeout = 0.85f;
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
    private List<Material> materials;
    // private Color color;
    private Animator animator;

    void Start() {
        transform.position += Vector3.up * upwardOffset;
        currentHealth = maxHealth;
        canMove = true;
        materials = new List<Material>();
        GetComponentsInChildren<Renderer>().ToList().ForEach(r => materials.AddRange(r.materials));
        animator = GetComponent<Animator>();
        animator.SetFloat("moveSpeedMult", speed * 0.5f);
    }

    void Update() {
        animator.SetBool("canMove", canMove);
        if (canMove)
            Move();
    }

    void Move() {
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
    }

    private IEnumerator Attack(Plant plant) {
        while (true) {
            // play attack animation
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackAnimationTimeout);
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
            animator.SetTrigger("Hit");
            SoundManager.instance.PlaySound(Sounds.ZombieHit);
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
            material.SetColor("_EmissionColor", Color.red);
        }

        yield return new WaitForSeconds(0.2f);

        foreach (var material in materials) {
            material.color = colors[0];
            colors.RemoveAt(0);
            material.SetColor("_EmissionColor", Color.white);
        }

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
        canMove = false;
        animator.SetTrigger("Dead");
        SoundManager.instance.PlaySound(Sounds.ZombieDie);
        // StopAllCoroutines();
        if (attackRoutine != null)
            StopCoroutine(attackRoutine);
        GetComponentsInChildren<Collider>().ToList().ForEach(c => c.enabled = false);
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Plant")) {
            canMove = false;
            var plant = other.GetComponent<Plant>();
            plant.died.AddListener(() => {
                // StopAllCoroutines();
                if (attackRoutine != null)
                    StopCoroutine(attackRoutine);
                canMove = true;
            });
            attackRoutine = Attack(plant);
            StartCoroutine(attackRoutine);
            // touchedPlant?.Invoke();
        }
    }
}
