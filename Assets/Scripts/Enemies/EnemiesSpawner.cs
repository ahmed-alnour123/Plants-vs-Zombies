using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesSpawner : MonoBehaviour {

    public float timeSleep;
    public List<GameObject> enemies;
    public List<Transform> spawnPoints;
    public Slider waveSlider;

    [HideInInspector]
    public bool canSpawn = true;

    void Start() {
        StartCoroutine(Spawn());
    }

    private void Update() {
        // waveSlider.value = waveCounter;
    }

    IEnumerator Spawn() {
        while (true) {
            if (canSpawn)
                Instantiate(Utils.RandomSelect<GameObject>(enemies), Utils.RandomSelect<Transform>(spawnPoints).position, Quaternion.identity);
            yield return new WaitForSeconds(timeSleep);
        }
    }
}
