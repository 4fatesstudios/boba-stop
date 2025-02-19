using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";

    [SerializeField] private Character character;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }
    
    private void Update() {
        animator.SetBool(IS_WALKING, character.IsWalking());
    }
}
