using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlantsPlacer : MonoBehaviour {
    [HideInInspector]
    public UnityEvent<Plant> plantPlaced = default;

    public LayerMask tilesLayer;
    public Camera cam;
    public List<Plant> plants;
    [HideInInspector]
    public Plant currentPlant;
    [Space()]
    public GameObject cardTemplate;
    public Transform cardsUIParent;
    public Gradient gradient;

    [HideInInspector] public bool isDragging;

    private GameManager gameManager;

    private void Start() {
        gameManager = GameManager.instance;

        foreach (var plant in plants) {
            var card = Instantiate(cardTemplate, cardsUIParent);
            card.SetActive(true);
            var drag = card.GetComponent<PlantDrag>();
            drag.plant = plant;
            drag.timeout = plant.cardSpawnTimeout;
            card.GetComponentInChildren<Image>().color = gradient.Evaluate(((float)plants.IndexOf(plant) / plants.Count));

            card.transform.Find("Container/Frame/Image").GetComponent<Image>().sprite = plant.icon;
            card.transform.Find("Container/Container/Price").GetComponent<TMP_Text>().text = "" + plant.price;
            // card.sprite = plant.sprite
        }
    }

    public void PlacePlant() {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        // if(gameManager.UseCoins(currentPlant)){
        //     return;
        // }

        if (Physics.Raycast(ray, out RaycastHit info, 1000, tilesLayer)) {
            // Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), info.transform.position + Vector3.up, Quaternion.identity).transform.localScale = Vector3.one * 0.25f;
            if (info.transform.gameObject.layer == LayerMask.NameToLayer("Tile") && TileIsEmpty(info.transform)) {
                // var newPlant = Instantiate(currentPlant, info.transform.position + Vector3.up * currentPlant.upwardOffset, Quaternion.identity, transform);
                var newPlant = Instantiate(currentPlant, transform);
                newPlant.transform.position = info.transform.position + Vector3.up * currentPlant.upwardOffset;
                newPlant.StartUseAbility();
                info.transform.GetComponent<Tile>().plant = newPlant;
                plantPlaced?.Invoke(currentPlant);
            }
            // if (info.transform.CompareTag("Plant"))
        }
    }

    private bool TileIsEmpty(Transform tileTransform) {
        var tile = tileTransform.GetComponent<Tile>();
        if (tile == null) return false;
        return tile.plant == null;
    }
}
