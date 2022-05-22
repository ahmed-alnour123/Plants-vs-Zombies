using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationPlayer : MonoBehaviour {
    public List<RuntimeAnimatorController> controllers;

    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();

        animator.runtimeAnimatorController = Utils.RandomSelect(controllers);
        animator.speed = Random.Range(0.5f, 1);
    }
}
