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
                Instantiate(RandomSelect<GameObject>(enemies), RandomSelect<Transform>(spawnPoints).position, Quaternion.identity);
            yield return new WaitForSeconds(timeSleep);
        }
    }

    T RandomSelect<T>(List<T> list) {
        var index = Random.Range(0, list.Count);
        return list[index];
    }

}
