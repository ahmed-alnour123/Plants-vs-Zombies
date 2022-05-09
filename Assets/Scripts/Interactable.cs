using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {

    private UnityEvent touchedPlant = default;
    private UnityEvent died = default;

    private void OnTriggerEnter(Collider other) {

    }
}
