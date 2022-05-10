using UnityEngine;

public class PlantsPlacer : MonoBehaviour {

    public LayerMask tilesLayer;
    public Camera cam;
    public GameObject plant;

    [HideInInspector] public bool isDragging;

    void Update() {

    }

    public void PlacePlant() {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit info, 1000, tilesLayer)) {
            // Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), info.transform.position + Vector3.up, Quaternion.identity).transform.localScale = Vector3.one * 0.25f;
            if (info.transform.gameObject.layer == LayerMask.NameToLayer("Tile") && TileIsEmpty(info.transform.position))
                Instantiate(plant, info.transform.position, Quaternion.identity, transform);
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
