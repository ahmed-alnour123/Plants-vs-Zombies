using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlantDrag : MonoBehaviour {
    public GameObject plant;
    private Camera cam;

    private PlantsPlacer placer;
    private GameObject currentPlant;

    // Debug

    private void Start() {
        placer = FindObjectOfType<PlantsPlacer>();
        cam = Camera.main;
    }

    private void Update() {
        if (currentPlant == null) return;
        var mousePos = Input.mousePosition;
        mousePos.z = 10;
        var newPos = cam.ScreenToWorldPoint(mousePos);
        currentPlant.transform.position = newPos;
    }

    public void MouseDown(BaseEventData data) {
        Debug.Log("Mouse Down");
        PointerEventData pointerData = (PointerEventData)data;
        if (placer.isDragging) return;
        placer.isDragging = true;

        if (currentPlant != null) {
            Destroy(currentPlant);
            throw new System.Exception("How did this happen?");
        }

        placer.plant = plant;
        currentPlant = Instantiate(plant);
        currentPlant.name = "HI THERE";
        currentPlant.transform.localScale = plant.transform.localScale;
        currentPlant.GetComponents<Collider>().ToList().ForEach(c => c.enabled = false);
    }

    public void MouseUp(BaseEventData data) {
        Debug.Log("Mouse Up");
        PointerEventData pointerData = (PointerEventData)data;
        placer.isDragging = false;

        if (currentPlant != null) {
            Destroy(currentPlant);
            placer.PlacePlant();
        } else {
            throw new System.Exception("How did you get here?");
        }
    }
}
