using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class PlantDrag : MonoBehaviour {
    public Plant plant;

    private Camera cam;
    private PlantsPlacer placer;
    private Plant currentPlant;
    private float upwardOffset;
    private GameManager gameManager;

    // Debug

    private void Start() {
        placer = FindObjectOfType<PlantsPlacer>();
        upwardOffset = plant.upwardOffset / 2;
        cam = Camera.main;
        gameManager = GameManager.instance;

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
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit info)) {
            currentPlant.transform.position = info.point + (Vector3.up * upwardOffset);
        } else {
            var mousePos = Input.mousePosition;
            mousePos.z = 20;
            currentPlant.transform.position = cam.ScreenToWorldPoint(mousePos);
        }
    }

    public void MouseDown(BaseEventData data) {
        if (placer.isDragging) return;

        if (!gameManager.UseCoins(plant.price)) return;

        placer.isDragging = true;

        if (currentPlant != null) {
            Destroy(currentPlant.gameObject);
            throw new System.Exception("How did this happen?");
        }

        placer.currentPlant = plant;
        currentPlant = Instantiate(plant);
        currentPlant.name = "HI THERE";
        currentPlant.transform.localScale = plant.transform.localScale;
        currentPlant.GetComponents<Collider>().ToList().ForEach(c => c.enabled = false);
    }

    public void MouseUp(BaseEventData data) {
        if (!placer.isDragging) return;

        placer.isDragging = false;

        if (currentPlant != null) {
            Destroy(currentPlant.gameObject);
            placer.PlacePlant();
        } else {
            throw new System.Exception("How did you get here?");
        }
    }
}
