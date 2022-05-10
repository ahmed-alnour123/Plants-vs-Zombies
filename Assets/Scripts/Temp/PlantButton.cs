using UnityEngine;
using UnityEngine.UI;

public class PlantButton : MonoBehaviour {

    public GameObject plant;

    private PlantsPlacer placer;

    void Start() {
        placer = FindObjectOfType<PlantsPlacer>();
        GetComponent<Button>().onClick.AddListener(() => { placer.plant = plant; });
    }


    void Update() {

    }
}
