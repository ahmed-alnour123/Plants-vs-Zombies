using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesSpawner : MonoBehaviour {

    public float betweenSpawnsTimeout;
    public float betweenWavesTimeout;

    public List<EnemyWave> waves;
    public List<Transform> spawnPoints;

    [Header("UI")]
    public Slider waveSlider;
    public WaveTextAnimation waveText;

    // private int enemiesCount;
    private int enemiesCount;
    private int waveEnemiesCount;
    private int waveNumber;
    private int index;
    private int sliderCount;
    private int totalEnemies;

    void Start() {
        // enemiesCount = waves.Sum();
        index = 0;
        enemiesCount = 0;
        foreach (var wave in waves) {
            foreach (var enemy in wave.enemies) {
                enemiesCount += enemy.count;
            }
        }
        sliderCount = 0;
        totalEnemies = enemiesCount;
        waveText.ShowText(++waveNumber == waves.Count ? "Final Wave" : "Wave " + waveNumber);
        StartCoroutine(Spawn());

    }

    IEnumerator Spawn(int index = 0) {
        yield return new WaitForSeconds(betweenWavesTimeout);
        var wave = waves[index];
        waveEnemiesCount = 0;

        foreach (var enemy in wave.enemies) {
            waveEnemiesCount += enemy.count;
        }

        while (wave.enemies.Count > 0) {
            var container = Utils.RandomSelect(wave.enemies);
            container.count--;
            if (container.count < 0) {
                wave.enemies.Remove(container);
                continue;
            }
            SoundManager.instance.PlaySound(Sounds.ZombieInstantiate);
            var enemy = Instantiate(container.enemy, Utils.RandomSelect(spawnPoints).position, Quaternion.Euler(Vector3.up * -90));
            sliderCount++;
            gameObject.LeanValue(waveSlider.value, (float)sliderCount / totalEnemies, 2).setOnUpdate(f => waveSlider.value = f);
            enemy.died.AddListener(() => ReduceEnemiesCount());
            yield return new WaitForSeconds(betweenSpawnsTimeout);
        }
    }

    private void ReduceEnemiesCount() {
        enemiesCount--;
        waveEnemiesCount--;

        if (waveEnemiesCount <= 0) {
            index++;
            if (index >= waves.Count) {
                GameManager.instance.WinGame();
            } else {
                waveText.ShowText(++waveNumber == waves.Count ? "Final Wave" : "Wave " + waveNumber);
                StartCoroutine(Spawn(index));
            }
        }
    }

    // IEnumerator Spawn(int i) {
    //     if (i == waves.Length - 1)
    //         lastWave = true;

    //     yield return new WaitForSeconds(betweenWavesTimeout);
    //     for (int j = 0; j < waves[i]; j++) {
    //         currentEnemiesCount++;
    //         gameObject.LeanValue(waveSlider.value, (float)currentEnemiesCount / enemiesCount, 2).setOnUpdate(f => waveSlider.value = f);
    //         var enemy = Instantiate(Utils.RandomSelect<GameObject>(enemies), Utils.RandomSelect<Transform>(spawnPoints).position, Quaternion.Euler(Vector3.up * -90));
    //         if (j == waves[i] - 1) // if last enemy
    //             enemy.GetComponent<Enemy>().died.AddListener(() => {
    //                 if (lastWave)
    //                     GameManager.instance.WinGame();
    //                 else
    //                     StartCoroutine(Spawn(i + 1));
    //             });
    //         yield return new WaitForSeconds(betweenSpawnsTimeout);
    //     }
    // }
}


[System.Serializable]
public class EnemyContainer {
    public Enemy enemy;
    [Range(0, 10)]
    public int count;
}

[System.Serializable]
public class EnemyWave {
    public List<EnemyContainer> enemies;
}
