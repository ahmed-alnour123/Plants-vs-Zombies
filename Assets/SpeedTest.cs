using UnityEngine;

public class SpeedTest : MonoBehaviour {
    [Range(0f, 1f)]
    public float speedMult;
    public float hitMult;

    private Enemy enemy;
    private Animator animator;

    void Start() {
        enemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        // animator.SetFloat("moveSpeedMult", speedMult * enemy.speed);
    }
}
