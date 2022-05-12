using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantsPlacer : MonoBehaviour {

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
            card.GetComponent<PlantDrag>().plant = plant;
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
            if (info.transform.gameObject.layer == LayerMask.NameToLayer("Tile") && TileIsEmpty(info.transform.position)) {
                var newPlant = Instantiate(currentPlant, info.transform.position + Vector3.up * currentPlant.upwardOffset, Quaternion.identity, transform);
                newPlant.StartUseAbility();
                info.transform.GetComponent<Tile>().plant = newPlant;
            }
            // if (info.transform.CompareTag("Plant"))
        }
    }

    private bool TileIsEmpty(Vector3 point) {
        foreach (var tileObject in Physics.OverlapSphere(point + Vector3.up, 0.5f)) {
            if (tileObject.tag == "Plant") {
                return false;
            }
        }
        return true;
    }
}