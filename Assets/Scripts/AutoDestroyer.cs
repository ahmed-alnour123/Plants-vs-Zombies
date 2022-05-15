using UnityEngine;

public class AutoDestroyer : MonoBehaviour {
    public float timeout = 2;
    void Start() {
        Destroy(gameObject, timeout);
    }
}
