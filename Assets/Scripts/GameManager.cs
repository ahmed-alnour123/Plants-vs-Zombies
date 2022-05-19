using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public int coins;
    public int health;
    public TMP_Text coinsText;
    public Toggle deleteToggle;
    public Texture2D deleteCursor;
    public UIShowAnimation winMenu;
    public UIShowAnimation loseMenu;
    public Image houseHealth;

    [HideInInspector]
    public bool isDeleting;

    private int initHealth;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        UpdateCoins();
        isDeleting = false;
        initHealth = health;
        Resume();
    }

    private void LoseGame() {
        Debug.Log("You Lost");
        Pause();
        loseMenu.Show();
    }

    public void WinGame() {
        Debug.Log("You Won");
        Pause();
        winMenu.Show();
    }

    public void AttackHouse(int amount = 1) {
        health -= amount;
        houseHealth.fillAmount = (float)health / initHealth;
        if (health <= 0)
            LoseGame();
    }

    public void AddCoins(int amount) {
        coins += amount;
        UpdateCoins();
    }

    public bool CanUse(int amount) {
        return coins >= amount;
    }

    public void UseCoins(int amount) {
        coins -= amount;
        UpdateCoins();
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

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnToggleChange(bool value) {
        isDeleting = !deleteToggle.isOn;
        Texture2D newCursor = null;
        if (isDeleting)
            newCursor = deleteCursor;
        Cursor.SetCursor(newCursor, Vector2.one * 8, CursorMode.Auto);
    }
}
