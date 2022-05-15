using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public int coins;
    public int health;
    public TMP_Text coinsText;
    public Toggle deleteToggle;
    public Texture2D deleteCursor;
    // [HideInInspector]
    public bool isDeleting;

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

    public void OnToggleChange(bool value) {
        isDeleting = !deleteToggle.isOn;
        Texture2D newCursor = null;
        if (isDeleting)
            newCursor = deleteCursor;
        Cursor.SetCursor(newCursor, Vector2.one * 8, CursorMode.Auto);
    }
}
