using UnityEngine;

public class Sun : MonoBehaviour {

    [HideInInspector]
    public int coins;

    private Vector3 initialPos;
    private GameManager gameManager;

    void Start() {
        initialPos = transform.position;
        gameManager = GameManager.instance;
    }


    void Update() {
        transform.position = initialPos + (Vector3.up * Mathf.Sin(Time.time) * 0.25f);
        transform.Rotate(30 * Vector3.up * Time.deltaTime);
    }

    private void OnMouseDown() {
        gameManager.AddCoins(coins);
        SoundManager.instance.PlaySound(Sounds.Coin);
        Destroy(gameObject);
    }
}
