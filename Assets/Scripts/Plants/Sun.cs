using UnityEngine;

public class Sun : MonoBehaviour {

    private Vector3 initialPos;
    void Start() {
        initialPos = transform.position;
    }


    void Update() {
        transform.position = initialPos + (Vector3.up * Mathf.Sin(Time.time) * 0.25f);
    }

    private void OnMouseDown() {
        Destroy(gameObject);
    }
}
