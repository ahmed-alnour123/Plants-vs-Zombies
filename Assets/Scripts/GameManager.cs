using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public int coins;
    public int health;
    public TMP_Text coinsText;
    [HideInInspector] public bool isDeleting;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        UpdateCoins();
        isDeleting = false;
    }

    private void LoseGame() {
        Debug.Log("You Lost");
    }

    public void WinGame() {
        Debug.Log("You Won");
    }

    public void AttackHouse() {
        health--;
        if (health <= 0)
            LoseGame();
    }

    public void AddCoins(int amount) {
        coins += amount;
        UpdateCoins();
    }

    public bool UseCoins(int amount) {
        if (coins < amount) return false;

        coins -= amount;
        UpdateCoins();
        return true;
    }

    private void UpdateCoins() {
        coinsText.text = "" + coins;
    }

    public void Resume() {
        Time.timeScale = 1;
    }

    public void Pause() {
        Time.timeScale = 0;
    }

    public void ReturnToMainMenu() {
        SceneManager.LoadScene("");
    }

    public void OnToggleChange(bool value) {
        isDeleting = !value;
    }
}
