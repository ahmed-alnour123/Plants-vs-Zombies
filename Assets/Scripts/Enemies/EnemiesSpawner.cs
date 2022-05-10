using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour {

    public float timeSleep;
    public List<GameObject> enemies;
    public List<Transform> spawnPoints;

    [HideInInspector]
    public bool canSpawn = true;

    void Start() {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn() {
        while (true) {
            if (canSpawn)
                Instantiate(Utils.RandomSelect<GameObject>(enemies), Utils.RandomSelect<Transform>(spawnPoints).position, Quaternion.identity);
            yield return new WaitForSeconds(timeSleep);
        }
    }
}
