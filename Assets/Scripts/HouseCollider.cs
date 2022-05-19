using UnityEngine;

public class HouseCollider : MonoBehaviour {
    private GameManager gameManager;

    void Start() {
        gameManager = GameManager.instance;
    }

    public void TakeDamage(int amount) {
        gameManager.AttackHouse();
    }

    // private void OnTriggerEnter(Collider other) {
    //     if (other.CompareTag("Enemy")) {
    //         // gameManager.AttackHouse();
    //     }
    // }
}

