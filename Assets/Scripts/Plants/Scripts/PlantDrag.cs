using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class PlantDrag : MonoBehaviour {
    public Plant plant;

    [HideInInspector]
    public int timeout;

    private Camera cam;
    private PlantsPlacer placer;
    private Plant currentPlant;
    private float upwardOffset;
    private GameManager gameManager;
    private Image blackFill;
    private bool isCounting;

    private void Start() {
        placer = FindObjectOfType<PlantsPlacer>();
        upwardOffset = plant.upwardOffset / 2;
        cam = Camera.main;
        gameManager = GameManager.instance;
        blackFill = transform.Find("Container/BlackFill").GetComponent<Image>();
        isCounting = false;
        placer.plantPlaced.AddListener(p => { if (p == plant) StartCounting(); });

        StartCounting();
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

    public void StartCounting() {
        if (!isCounting)
            StartCoroutine(StartCountingRoutine());
    }

    IEnumerator StartCountingRoutine() {
        isCounting = true;
        for (float i = 0; i < timeout; i += 0.1f) {
            blackFill.fillAmount = 1 - ((float)i / timeout);
            yield return new WaitForSeconds(0.1f);
        }
        blackFill.fillAmount = 0;
        isCounting = false;
    }

    public void MouseDown(BaseEventData data) {
        if (placer.isDragging || isCounting) return;

        if (!gameManager.CanUse(plant.price)) return;

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
            gameManager.UseCoins(currentPlant.price);
            placer.PlacePlant();
        } else {
            throw new System.Exception("How did you get here?");
        }
    }
}
