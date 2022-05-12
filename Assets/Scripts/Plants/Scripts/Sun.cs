using UnityEngine;

public class Sun : MonoBehaviour {

    public int coins;

    private Vector3 initialPos;
    private GameManager gameManager;

    void Start() {
        initialPos = transform.position;
        gameManager = GameManager.instance;
    }


    void Update() {
        transform.position = initialPos + (Vector3.up * Mathf.Sin(Time.time) * 0.25f);
    }

    private void OnMouseDown() {
        gameManager.AddCoins(coins);
        Destroy(gameObject);
    }
}
