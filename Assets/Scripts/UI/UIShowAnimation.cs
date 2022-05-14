using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(CanvasGroup))]
public class UIShowAnimation : MonoBehaviour {
    public float time = 0.5f;
    public LeanTweenType easeInOut = LeanTweenType.easeOutBack;

    private CanvasGroup canvasGroup;

    private void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show() {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        LeanTween.scale(gameObject, Vector3.one, time).setEase(LeanTweenType.easeOutBack).setIgnoreTimeScale(true);
    }

    public void Hide() {
        LeanTween.scale(gameObject, Vector3.zero, time).setEase(LeanTweenType.easeInBack).setOnComplete(() => {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
        });
    }
}
