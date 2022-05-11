using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantsPlacer : MonoBehaviour {

    public LayerMask tilesLayer;
    public Camera cam;
    public List<GameObject> plants;
    [HideInInspector]
    public GameObject currentPlant;
    [Space()]
    public GameObject cardTemplate;
    public Transform cardsUIParent;
    public Gradient gradient;

    [HideInInspector] public bool isDragging;

    private void Start() {
        foreach (var plant in plants) {
            var card = Instantiate(cardTemplate, cardsUIParent);
            card.SetActive(true);
            card.GetComponent<PlantDrag>().plant = plant;
            card.GetComponentInChildren<Image>().color = gradient.Evaluate(((float)plants.IndexOf(plant) / plants.Count));
            // card.sprite = plant.sprite
        }
    }

    void Update() {

    }

    public void PlacePlant() {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit info, 1000, tilesLayer)) {
            // Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), info.transform.position + Vector3.up, Quaternion.identity).transform.localScale = Vector3.one * 0.25f;
            if (info.transform.gameObject.layer == LayerMask.NameToLayer("Tile") && TileIsEmpty(info.transform.position))
                Instantiate(currentPlant, info.transform.position + Vector3.up * currentPlant.GetComponent<Plant>().upwardOffset, Quaternion.identity, transform).GetComponent<Plant>().StartUseAbility();
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
