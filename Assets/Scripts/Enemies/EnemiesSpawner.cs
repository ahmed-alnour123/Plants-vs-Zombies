using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesSpawner : MonoBehaviour {

    public int[] waves;
    public float betweenWavesTimeout;
    public float betweenSpawnsTimeout;

    public List<GameObject> enemies;
    public List<Transform> spawnPoints;

    [Header("UI")]
    public Slider waveSlider;
    public TMP_Text level;

    private bool lastWave;

    void Start() {
        StartCoroutine(Spawn(0));
    }

    private void Update() {
        // waveSlider.value = waveCounter;
    }

    IEnumerator Spawn(int i) {
        if (i == waves.Length - 1)
            lastWave = true;

        yield return new WaitForSeconds(betweenWavesTimeout);
        for (int j = 0; j < waves[i]; j++) {
            var enemy = Instantiate(Utils.RandomSelect<GameObject>(enemies), Utils.RandomSelect<Transform>(spawnPoints).position, Quaternion.identity);
            if (j == waves[i] - 1)
                enemy.GetComponent<Enemy>().died.AddListener(() => {
                    if (lastWave)
                        GameManager.instance.WinGame();
                    else
                        StartCoroutine(Spawn(i + 1));
                });
            yield return new WaitForSeconds(betweenSpawnsTimeout);
        }
    }
}
