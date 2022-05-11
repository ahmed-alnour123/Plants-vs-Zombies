using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class PlantDrag : MonoBehaviour {
    public GameObject plant;
    private Camera cam;

    private PlantsPlacer placer;
    private GameObject currentPlant;
    private float upwardOffset;

    // Debug

    private void Start() {
        placer = FindObjectOfType<PlantsPlacer>();
        var plantComponent = plant.GetComponent<Plant>();
        upwardOffset = plantComponent.upwardOffset / 2;
        cam = Camera.main;

        // Mouse up
        var trigger = GetComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener(MouseDown);
        trigger.triggers.Add(entry);

        // Mouse down
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener(MouseUp);
        trigger.triggers.Add(entry);
    }

    private void Update() {
        if (currentPlant == null) return;
        // var mousePos = Input.mousePosition;
        // mousePos.z = 10;
        // currentPlant.transform.position = cam.ScreenToWorldPoint(mousePos);
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit info)) {
            currentPlant.transform.position = info.point + (Vector3.up * upwardOffset);
        }
    }

    public void MouseDown(BaseEventData data) {
        PointerEventData pointerData = (PointerEventData)data;
        if (placer.isDragging) return;
        placer.isDragging = true;

        if (currentPlant != null) {
            Destroy(currentPlant);
            throw new System.Exception("How did this happen?");
        }

        placer.currentPlant = plant;
        currentPlant = Instantiate(plant);
        currentPlant.name = "HI THERE";
        currentPlant.transform.localScale = plant.transform.localScale;
        currentPlant.GetComponents<Collider>().ToList().ForEach(c => c.enabled = false);
    }

    public void MouseUp(BaseEventData data) {
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
