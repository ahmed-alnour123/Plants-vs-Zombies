using UnityEngine;

public class Tile : MonoBehaviour {
    public Plant plant;

    private GameManager gameManager;
    private PlantsPlacer placer;
    private new Renderer renderer;
    private Color originalColor;

    void Start() {
        gameManager = GameManager.instance;
        placer = FindObjectOfType<PlantsPlacer>();
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    private void OnMouseDown() {
        if (gameManager.isDeleting && plant != null) {
            gameManager.AddCoins(plant.price / 2);
            Destroy(plant.gameObject);
        }
    }

    private void OnMouseEnter() {
        if (placer.isDragging) {
            renderer.material.color = Color.grey;
        }
    }

    private void OnMouseExit() {
        renderer.material.color = originalColor;
    }

}
