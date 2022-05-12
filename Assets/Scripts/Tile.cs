using UnityEngine;

public class Tile : MonoBehaviour {
    public Plant plant;

    private GameManager gameManager;

    void Start() {
        gameManager = GameManager.instance;
    }

    private void OnMouseDown() {
        if (gameManager.isDeleting && plant != null) {
            gameManager.AddCoins(plant.price / 2);
            Destroy(plant.gameObject);
        }
    }
}
