using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveTextAnimation : MonoBehaviour {
    public float time;
    public Vector3 newScale;

    private CanvasGroup canvasGroup;
    private TMP_Text text;

    private void OnEnable() {
        canvasGroup = GetComponent<CanvasGroup>();

        transform.localScale = Vector3.zero;
        canvasGroup.alpha = 1;

        gameObject.LeanScale(newScale, time).setEaseOutBack().setOnComplete(
           () => gameObject.LeanValue(1, 0, time / 3)
           .setOnUpdate(f => canvasGroup.alpha = f)
           .setOnComplete(() => gameObject.SetActive(false))
        );
    }

    public void ShowText(string Text) {
        text = GetComponent<TMP_Text>();
        text.text = Text;
        gameObject.SetActive(true);
    }
}
