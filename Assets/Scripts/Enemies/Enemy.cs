using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour {
    private UnityEvent touchedPlant = default;
    private UnityEvent died = default;

    public int maxHealth;
    public float speed;
    public float upwardOffset;

    private int currentHealth;

    void Start() {
        transform.position += Vector3.up * upwardOffset;
        currentHealth = maxHealth;
    }


    void Update() {
        Move();
    }

    void Move() {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Plant")) {

        }
    }
}
