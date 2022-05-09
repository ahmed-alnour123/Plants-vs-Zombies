using UnityEngine;
using UnityEngine.Events;

public class Plant : MonoBehaviour {
    private UnityEvent touchedEnemy = default;
    private UnityEvent died = default;

    public float upwardOffset;

    void Start() {
        transform.position += Vector3.up * upwardOffset;
    }


    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            touchedEnemy?.Invoke();
        }
    }
}
