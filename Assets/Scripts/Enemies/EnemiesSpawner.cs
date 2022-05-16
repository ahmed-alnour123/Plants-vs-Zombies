using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private int enemiesCount;
    private int currentEnemiesCount;

    void Start() {
        StartCoroutine(Spawn(0));
        enemiesCount = waves.Sum();
        currentEnemiesCount = 0;
    }

    IEnumerator Spawn(int i) {
        if (i == waves.Length - 1)
            lastWave = true;

        yield return new WaitForSeconds(betweenWavesTimeout);
        for (int j = 0; j < waves[i]; j++) {
            currentEnemiesCount++;
            gameObject.LeanValue(waveSlider.value, (float)currentEnemiesCount / enemiesCount, 2).setOnUpdate(f => waveSlider.value = f);
            var enemy = Instantiate(Utils.RandomSelect<GameObject>(enemies), Utils.RandomSelect<Transform>(spawnPoints).position, Quaternion.Euler(Vector3.up * -90));
            if (j == waves[i] - 1) // if last enemy
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
